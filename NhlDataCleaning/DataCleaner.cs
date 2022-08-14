using Entities;
using Entities.Models;
using Microsoft.Extensions.Logging;
using NhlDataCleaning.Mappers;
using DataAccess.GameRepository;
using DataAccess.PlayerRepository;
using NhlDataCleaning.RequestMaker;

namespace NhlDataCleaning
{
    public class DataCleaner
    {
        private IPlayerRepository _playerRepo;
        private IGameRepository _gameRepo;
        private IRosterRequestMaker _rosterRequestMaker;
        private readonly ILogger _logger;
        private readonly DateRange _yearRange;
        private const int CUTOFF_COUNT = 300;
        private const int RECENT_GAMES = 5;
        private const int GAMES_TO_EXCLUDE = 15;

        public DataCleaner(ILogger logger, IPlayerRepository playerRepo, IGameRepository gameRepo, IRosterRequestMaker rosterRequestMaker, DateRange dateRange)
        {
            _logger = logger;
            _yearRange = dateRange;
            _gameRepo = gameRepo;
            _playerRepo = playerRepo;
            _rosterRequestMaker = rosterRequestMaker;
        }
        public async Task CleanData()
        {
            List<CleanedGame> games;
            List<Game> seasonsGames;
            List<Game> lastSeasonsGames;
            // Get and clean games
            for (int year = _yearRange.StartYear; year <= _yearRange.EndYear; year++)
            {
                // Skip year if its already been collected, always run current year
                var gameCount = await _gameRepo.GetCleanedGameCountBySeason(year);
                if (gameCount > CUTOFF_COUNT && year < _yearRange.EndYear)
                    continue;

                _logger.LogInformation("Cleaning Year: " + year.ToString());
                await _gameRepo.CacheSeasonOfGames(year);
                await _gameRepo.CacheLastSeasonOfGames(year - 1);
                seasonsGames = _gameRepo.GetCachedSeasonsGames();
                lastSeasonsGames = _gameRepo.GetCachedLastSeasonsGames();
                games = await CleanGames(seasonsGames, lastSeasonsGames);
                games = await GetRosterValues(games);
                await _gameRepo.AddCleanedGames(games);
            }
        }

        private async Task<List<CleanedGame>> GetRosterValues(List<CleanedGame> games)
        {
            await _playerRepo.CachePlayerValues();
            // No foreach because I update game
            for(int i = 0; i < games.Count(); i++)
            {
                var game = games[i];
                var ids = await _rosterRequestMaker.GetPlayerIds(game.id);
                foreach(var id in ids.homeRosterIds)
                {
                    var position = _playerRepo.GetPositionByIdFromCache(id);
                    var value = _playerRepo.GetPlayerValueByIdFromCache(id);
                    if (position == "D")
                        game.homeRosterDefenseValue += value;
                    else if (position == "G")
                        game.homeRosterGoalieValue += value;
                    else
                        game.homeRosterOffenseValue += value;
                }
                foreach(var id in ids.awayRosterIds)
                {
                    var position = _playerRepo.GetPositionByIdFromCache(id);
                    var value = _playerRepo.GetPlayerValueByIdFromCache(id);
                    if (position == "D")
                        game.awayRosterDefenseValue += value;
                    else if (position == "G")
                        game.awayRosterGoalieValue += value;
                    else
                        game.awayRosterOffenseValue += value;
                }
            }
            return games;
        }

        private List<int> GetSeasonIds(List<Game> seasonsGames)
        {
            var ids = new List<int>();
            foreach (var game in seasonsGames)
                ids.Add(game.id);
            return ids;
        }

        private async Task<List<CleanedGame>> CleanGames(List<Game> seasonsGames, List<Game> lastSeasonsGames)
        {
            var cleanedGames = new List<CleanedGame>();
            seasonsGames = seasonsGames.OrderBy(i => i.gameDate).Reverse().ToList();
            lastSeasonsGames = lastSeasonsGames.OrderBy(i => i.gameDate).Reverse().ToList();
            foreach (var game in seasonsGames)
            {
                if (await CleanedGameExists(game))
                    continue;
                var cleanedGame = GetCleanGame(seasonsGames, lastSeasonsGames, game);
                cleanedGames.Add(cleanedGame);
            }

            return cleanedGames;
        }

        private CleanedGame GetCleanGame(List<Game> seasonsGames, List<Game> lastSeasonsGames, Game game)
        {
            var homeGames = GetTeamGames(seasonsGames, game.homeTeamId, game.gameDate);
            var awayGames = GetTeamGames(seasonsGames, game.awayTeamId, game.gameDate);
            bool isExcluded = false;
            List<Game> lastSeasonHomeGames;
            List<Game> lastSeasonAwayGames;
            var homeHoursBetweenGames = Cleaner.GetHoursBetweenLastTwoGames(homeGames);
            var awayHoursBetweenGames = Cleaner.GetHoursBetweenLastTwoGames(awayGames);


            if (homeGames.Count() < GAMES_TO_EXCLUDE)
            {
                lastSeasonHomeGames = GetTeamGames(lastSeasonsGames, game.homeTeamId, game.gameDate);
                homeGames = lastSeasonHomeGames.Concat(homeGames).ToList();
                isExcluded = true;
            }
            if (awayGames.Count() < GAMES_TO_EXCLUDE)
            {
                lastSeasonAwayGames = GetTeamGames(lastSeasonsGames, game.awayTeamId, game.gameDate);
                awayGames = lastSeasonAwayGames.Concat(awayGames).ToList();
                isExcluded = true;
            }

            var cleanedGame = new CleanedGame()
            {
                id = game.id,
                homeTeamId = game.homeTeamId,
                awayTeamId = game.awayTeamId,
                seasonStartYear = game.seasonStartYear,
                gameDate = game.gameDate,

                homeWinRatio = Cleaner.GetWinRatioOfRecentGames(homeGames, game.homeTeamId, homeGames.Count()),
                homeRecentWinRatio = Cleaner.GetWinRatioOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeGoalsAvg = Cleaner.GetGoalsAvgOfRecentGames(homeGames, game.homeTeamId, homeGames.Count()),
                homeRecentGoalsAvg = Cleaner.GetGoalsAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeConcededGoalsAvg = Cleaner.GetConcededGoalsAvgOfRecentGames(homeGames, game.homeTeamId, homeGames.Count()),
                homeRecentConcededGoalsAvg = Cleaner.GetConcededGoalsAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeRecentSogAvg = Cleaner.GetSogAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeRecentBlockedShotsAvg = Cleaner.GetBlockedShotsAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeRecentPpgAvg = Cleaner.GetPpgAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeRecentHitsAvg = Cleaner.GetHitsAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeRecentPimAvg = Cleaner.GetPimAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeRecentTakeawaysAvg = Cleaner.GetTakeawaysAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeRecentGiveawaysAvg = Cleaner.GetGiveawaysAvgOfRecentGames(homeGames, game.homeTeamId, RECENT_GAMES),
                homeConcededGoalsAvgAtHome = Cleaner.GetConcededGoalsAvgOfRecentGamesAtHome(homeGames, game.homeTeamId, homeGames.Count()),
                homeRecentConcededGoalsAvgAtHome = Cleaner.GetConcededGoalsAvgOfRecentGamesAtHome(homeGames, game.homeTeamId, RECENT_GAMES),
                homeGoalsAvgAtHome = Cleaner.GetGoalsAvgOfRecentGamesAtHome(homeGames, game.homeTeamId, homeGames.Count()),
                homeRecentGoalsAvgAtHome = Cleaner.GetGoalsAvgOfRecentGamesAtHome(homeGames, game.homeTeamId, RECENT_GAMES),
                homeHoursSinceLastGame = homeHoursBetweenGames,

                awayWinRatio = Cleaner.GetWinRatioOfRecentGames(awayGames, game.awayTeamId, awayGames.Count()),
                awayRecentWinRatio = Cleaner.GetWinRatioOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayGoalsAvg = Cleaner.GetGoalsAvgOfRecentGames(awayGames, game.awayTeamId, awayGames.Count()),
                awayRecentGoalsAvg = Cleaner.GetGoalsAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayConcededGoalsAvg = Cleaner.GetConcededGoalsAvgOfRecentGames(awayGames, game.awayTeamId, awayGames.Count()),
                awayRecentConcededGoalsAvg = Cleaner.GetConcededGoalsAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayRecentSogAvg = Cleaner.GetSogAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayRecentBlockedShotsAvg = Cleaner.GetBlockedShotsAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayRecentPpgAvg = Cleaner.GetPpgAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayRecentHitsAvg = Cleaner.GetHitsAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayRecentPimAvg = Cleaner.GetPimAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayRecentTakeawaysAvg = Cleaner.GetTakeawaysAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayRecentGiveawaysAvg = Cleaner.GetGiveawaysAvgOfRecentGames(awayGames, game.awayTeamId, RECENT_GAMES),
                awayConcededGoalsAvgAtAway = Cleaner.GetConcededGoalsAvgOfRecentGamesAtAway(awayGames, game.awayTeamId, awayGames.Count()),
                awayRecentConcededGoalsAvgAtAway = Cleaner.GetConcededGoalsAvgOfRecentGamesAtAway(awayGames, game.awayTeamId, RECENT_GAMES),
                awayGoalsAvgAtAway = Cleaner.GetGoalsAvgOfRecentGamesAtAway(awayGames, game.awayTeamId, awayGames.Count()),
                awayRecentGoalsAvgAtAway = Cleaner.GetGoalsAvgOfRecentGamesAtAway(awayGames, game.awayTeamId, RECENT_GAMES),
                awayHoursSinceLastGame = awayHoursBetweenGames,
                winner = game.winner,
                isExcluded = isExcluded,
                hasBeenPlayed = game.hasBeenPlayed,
            };
            return cleanedGame;
        }

        private async Task<bool> CleanedGameExists(Game game)
        {
            return await _gameRepo.GetIfCleanedGameExistsById(game.id);
        }
        private List<Game> GetTeamGames(List<Game> seasonsGames, int teamId, DateTime currentGameDate)
        {
            // Get games that happened before current game and include the team
            return seasonsGames.Where(i => i.gameDate < currentGameDate)
                .Where(n => n.awayTeamId == teamId || n.homeTeamId == teamId).ToList();
        }
    }
}
