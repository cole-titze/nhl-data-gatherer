using Entities;
using Entities.Models;
using Microsoft.Extensions.Logging;
using DataAccess.GamesRepository;
using System.Linq;

namespace NhlDataCleaning
{
    public class DataCleaner
    {
        private IGamesDA _gamesDA;
        private readonly ILogger _logger;
        private readonly DateRange _yearRange;

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
                var cleanedGame = new CleanedGame()
                {
                    id = game.id,
                    homeTeamName = game.homeTeamName,
                    awayTeamName = game.awayTeamName,
                    seasonStartYear = game.seasonStartYear,
                };
            }


            throw new NotImplementedException();
        }
    }
}
