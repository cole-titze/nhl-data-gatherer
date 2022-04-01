using Entities;
using Entities.Models;
using Microsoft.Extensions.Logging;
using DataAccess.GamesRepository;
using System.Linq;
using NhlDataCleaning.Mappers;

namespace NhlDataCleaning
{
    public class DataCleaner
    {
        private IGamesDA _gamesDA;
        private readonly ILogger _logger;
        private readonly DateRange _yearRange;
        private const int RECENT_GAMES = 5;

        public DataCleaner(ILogger logger, IGamesDA gamesDa, DateRange dateRange)
        {
            _logger = logger;
            _gamesDA = gamesDa;
            _yearRange = dateRange;
        }
        public void CleanData()
        {
            for (int year = _yearRange.StartYear; year <= _yearRange.EndYear; year++)
            {
                _logger.LogInformation("Cleaning Year: " + year.ToString());
                _gamesDA.CacheSeasonOfGames(year);
                var seasonsGames = _gamesDA.GetCachedGames();
                var games = CleanGames(seasonsGames);
            }
        }
        private List<CleanedGame> CleanGames(List<Game> seasonsGames)
        {
            var cleanedGames = new List<CleanedGame>();
            seasonsGames.OrderBy(i => i.id).Reverse();
            foreach(var game in seasonsGames)
            {
                var homeGames = GetTeamGames(seasonsGames, game.homeTeamName, game.id);
                var awayGames = GetTeamGames(seasonsGames, game.awayTeamName, game.id);
                var cleanedGame = new CleanedGame()
                {
                    id = game.id,
                    homeTeamName = game.homeTeamName,
                    awayTeamName = game.awayTeamName,
                    seasonStartYear = game.seasonStartYear,
                    homeWinRatio = Cleaner.GetWinRatioOfRecentGames(homeGames, game.homeTeamName, homeGames.Count()),
                    homeRecentWinRatio = Cleaner.GetWinRatioOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),

                    awayWinRatio = Cleaner.GetWinRatioOfRecentGames(awayGames, game.awayTeamName, awayGames.Count()),
                    awayRecentWinRatio = Cleaner.GetWinRatioOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),

                };
                cleanedGames.Add(cleanedGame);
            }

            return cleanedGames;
        }
        // Make list extension
        private List<Game> GetTeamGames(List<Game> seasonsGames, string teamName, int id)
        {
            // Get games that happened before current game and include the team
            return seasonsGames.Where(i => i.id < id)
                .Where(n => n.awayTeamName == teamName || n.homeTeamName == teamName).ToList();
        }
    }
}
