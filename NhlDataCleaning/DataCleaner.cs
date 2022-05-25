using Entities;
using Entities.Models;
using Microsoft.Extensions.Logging;
using NhlDataCleaning.Mappers;
using DataAccess.GameRepository;
using DataAccess.PlayerRepository;

namespace NhlDataCleaning
{
    public class DataCleaner
    {
        private IPlayerRepository _playerRepo;
        private IGameRepository _gameRepo;
        private readonly ILogger _logger;
        private readonly DateRange _yearRange;
        private const int RECENT_GAMES = 5;
        private const int GAMES_TO_EXCLUDE = 15;

        public DataCleaner(ILogger logger, IPlayerRepository playerRepo, IGameRepository gameRepo, DateRange dateRange)
        {
            _logger = logger;
            _yearRange = dateRange;
            _gameRepo = gameRepo;
            _playerRepo = playerRepo;
        }
        public async Task CleanData()
        {
            List<CleanedGame> games;
            List<Game> seasonsGames;
            // Get and clean games
            for (int year = _yearRange.StartYear; year <= _yearRange.EndYear; year++)
            {
                _logger.LogInformation("Cleaning Year: " + year.ToString());
                await _gameRepo.CacheSeasonOfGames(year);
                seasonsGames = _gameRepo.GetCachedGames();
                games = await CleanGames(seasonsGames);
                await _gameRepo.AddCleanedGames(games);
            }
            // Get and create future game records
            // TODO: I'm redoing the last seasons work here. Should be included above or something
            var futureGames = await _gameRepo.GetFutureGames();
            seasonsGames = _gameRepo.GetCachedGames();
            var ids = GetSeasonIds(seasonsGames);
            foreach(var game in futureGames)
            {
                var mappedGame = GameMapper.FutureGameToGame(game);
                if(!ids.Contains(mappedGame.id))
                    seasonsGames.Add(mappedGame);
            }
            var futureCleanedGames = await CleanFutureGames(seasonsGames);
            await _gameRepo.AddFutureCleanedGames(futureCleanedGames);
        }

        private List<int> GetSeasonIds(List<Game> seasonsGames)
        {
            var ids = new List<int>();
            foreach (var game in seasonsGames)
                ids.Add(game.id);
            return ids;
        }

        private async Task<List<CleanedGame>> CleanGames(List<Game> seasonsGames)
        {
            var cleanedGames = new List<CleanedGame>();
            seasonsGames = seasonsGames.OrderBy(i => i.id).Reverse().ToList();
            foreach(var game in seasonsGames)
            {
                if (await CleanedGameExists(game))
                    continue;
                var cleanedGame = GetCleanGame(seasonsGames, game);
                cleanedGames.Add(cleanedGame);
            }

            return cleanedGames;
        }
        private async Task<List<FutureCleanedGame>> CleanFutureGames(List<Game> seasonsGames)
        {
            var cleanedGames = new List<FutureCleanedGame>();
            seasonsGames = seasonsGames.OrderBy(i => i.id).Reverse().ToList();
            foreach (var game in seasonsGames)
            {
                if (await FutureCleanedGameExists(game))
                    continue;
                var cleanedGame = GetFutureCleanGame(seasonsGames, game);
                cleanedGames.Add(cleanedGame);
            }

            return cleanedGames;
        }

        private CleanedGame GetCleanGame(List<Game> seasonsGames, Game game)
        {
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
            return cleanedGame;
        }

        private FutureCleanedGame GetFutureCleanGame(List<Game> seasonsGames, Game game)
        {
            var homeGames = GetTeamGames(seasonsGames, game.homeTeamName, game.id);
            var awayGames = GetTeamGames(seasonsGames, game.awayTeamName, game.id);
            var cleanedGame = new FutureCleanedGame()
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
            };
            return cleanedGame;
        }


        private async Task<bool> CleanedGameExists(Game game)
        {
            return await _gameRepo.GetIfCleanedGameExistsById(game.id);
        }
        private async Task<bool> FutureCleanedGameExists(Game game)
        {
            return await _gameRepo.GetIfFutureCleanedGameExistsById(game.id);
        }
        private List<Game> GetTeamGames(List<Game> seasonsGames, string teamName, int id)
        {
            // Get games that happened before current game and include the team
            return seasonsGames.Where(i => i.id < id)
                .Where(n => n.awayTeamName == teamName || n.homeTeamName == teamName).ToList();
        }
    }
}
