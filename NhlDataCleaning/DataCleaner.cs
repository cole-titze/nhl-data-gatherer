using Entities;
using Entities.Models;
using Microsoft.Extensions.Logging;
using DataAccess.GamesRepository;
using DataAccess.CleanedGamesRepository;
using NhlDataCleaning.Mappers;

namespace NhlDataCleaning
{
    public class DataCleaner
    {
        private IGamesDA _gamesDA;
        private ICleanedGamesDA _cleanedGamesDA;
        private readonly ILogger _logger;
        private readonly DateRange _yearRange;
        private const int RECENT_GAMES = 5;
        private const int GAMES_TO_EXCLUDE = 15;

        public DataCleaner(ILogger logger, IGamesDA gamesDa, ICleanedGamesDA cleanedDA, DateRange dateRange)
        {
            _logger = logger;
            _gamesDA = gamesDa;
            _yearRange = dateRange;
            _cleanedGamesDA = cleanedDA;
        }
        public void CleanData()
        {
            // Get and clean games
            for (int year = _yearRange.StartYear; year <= _yearRange.EndYear; year++)
            {
                _logger.LogInformation("Cleaning Year: " + year.ToString());
                _gamesDA.CacheSeasonOfGames(year);
                var seasonsGames = _gamesDA.GetCachedGames();
                var games = CleanGames(seasonsGames);
                _cleanedGamesDA.AddGames(games);
            }
            // Get and create future game records
            
        }
        private List<CleanedGame> CleanGames(List<Game> seasonsGames)
        {
            _cleanedGamesDA.CacheGameIds();
            var cleanedGames = new List<CleanedGame>();
            seasonsGames = seasonsGames.OrderBy(i => i.id).Reverse().ToList();
            foreach(var game in seasonsGames)
            {
                if (GameExists(game))
                    continue;
                var homeGames = GetTeamGames(seasonsGames, game.homeTeamName, game.id);
                var awayGames = GetTeamGames(seasonsGames, game.awayTeamName, game.id);
                var cleanedGame = new CleanedGame()
                {
                    id = game.id,
                    homeTeamName = game.homeTeamName,
                    awayTeamName = game.awayTeamName,
                    seasonStartYear = game.seasonStartYear,
                    gameDate = game.gameDate,

                    homeWinRatio = Cleaner.GetWinRatioOfRecentGames(homeGames, game.homeTeamName, homeGames.Count()),
                    homeRecentWinRatio = Cleaner.GetWinRatioOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeGoalsAvg = Cleaner.GetGoalsAvgOfRecentGames(homeGames, game.homeTeamName, homeGames.Count()),
                    homeRecentGoalsAvg = Cleaner.GetGoalsAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeConcededGoalsAvg = Cleaner.GetConcededGoalsAvgOfRecentGames(homeGames, game.homeTeamName, homeGames.Count()),
                    homeRecentConcededGoalsAvg = Cleaner.GetConcededGoalsAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeRecentSogAvg = Cleaner.GetSogAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeRecentBlockedShotsAvg = Cleaner.GetBlockedShotsAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeRecentPpgAvg = Cleaner.GetPpgAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeRecentHitsAvg = Cleaner.GetHitsAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeRecentPimAvg = Cleaner.GetPimAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeRecentTakeawaysAvg = Cleaner.GetTakeawaysAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeRecentGiveawaysAvg = Cleaner.GetGiveawaysAvgOfRecentGames(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeConcededGoalsAvgAtHome = Cleaner.GetConcededGoalsAvgOfRecentGamesAtHome(homeGames, game.homeTeamName, homeGames.Count()),
                    homeRecentConcededGoalsAvgAtHome = Cleaner.GetConcededGoalsAvgOfRecentGamesAtHome(homeGames, game.homeTeamName, RECENT_GAMES),
                    homeGoalsAvgAtHome = Cleaner.GetGoalsAvgOfRecentGamesAtHome(homeGames, game.homeTeamName, homeGames.Count()),
                    homeRecentGoalsAvgAtHome = Cleaner.GetGoalsAvgOfRecentGamesAtHome(homeGames, game.homeTeamName, RECENT_GAMES),

                    awayWinRatio = Cleaner.GetWinRatioOfRecentGames(awayGames, game.awayTeamName, awayGames.Count()),
                    awayRecentWinRatio = Cleaner.GetWinRatioOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayGoalsAvg = Cleaner.GetGoalsAvgOfRecentGames(awayGames, game.awayTeamName, awayGames.Count()),
                    awayRecentGoalsAvg = Cleaner.GetGoalsAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayConcededGoalsAvg = Cleaner.GetConcededGoalsAvgOfRecentGames(awayGames, game.awayTeamName, awayGames.Count()),
                    awayRecentConcededGoalsAvg = Cleaner.GetConcededGoalsAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayRecentSogAvg = Cleaner.GetSogAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayRecentBlockedShotsAvg = Cleaner.GetBlockedShotsAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayRecentPpgAvg = Cleaner.GetPpgAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayRecentHitsAvg = Cleaner.GetHitsAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayRecentPimAvg = Cleaner.GetPimAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayRecentTakeawaysAvg = Cleaner.GetTakeawaysAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayRecentGiveawaysAvg = Cleaner.GetGiveawaysAvgOfRecentGames(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayConcededGoalsAvgAtAway = Cleaner.GetConcededGoalsAvgOfRecentGamesAtAway(awayGames, game.awayTeamName, awayGames.Count()),
                    awayRecentConcededGoalsAvgAtAway = Cleaner.GetConcededGoalsAvgOfRecentGamesAtAway(awayGames, game.awayTeamName, RECENT_GAMES),
                    awayGoalsAvgAtAway = Cleaner.GetGoalsAvgOfRecentGamesAtAway(awayGames, game.awayTeamName, awayGames.Count()),
                    awayRecentGoalsAvgAtAway = Cleaner.GetGoalsAvgOfRecentGamesAtAway(awayGames, game.awayTeamName, RECENT_GAMES),

                    winner = game.winner,
                    isExcluded = Cleaner.GetIsExcluded(awayGames, homeGames, GAMES_TO_EXCLUDE),
                };
                cleanedGames.Add(cleanedGame);
            }

            return cleanedGames;
        }

        private bool GameExists(Game game)
        {
            return _cleanedGamesDA.GetIfGameExistsByIdFromCache(game.id);
        }

        private List<Game> GetTeamGames(List<Game> seasonsGames, string teamName, int id)
        {
            // Get games that happened before current game and include the team
            return seasonsGames.Where(i => i.id < id)
                .Where(n => n.awayTeamName == teamName || n.homeTeamName == teamName).ToList();
        }
    }
}
