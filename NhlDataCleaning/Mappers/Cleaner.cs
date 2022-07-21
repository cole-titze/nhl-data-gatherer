using Entities.Models;

namespace NhlDataCleaning.Mappers
{
    public static class Cleaner
    {
        // If no game has been played set default as 4 days of rest (season hasn't started)
        public static readonly int DEFAULT_DAYS = 4;
        public static double GetWinRatioOfRecentGames(List<Game> teamSeasonGames, int teamId, int numberOfGames)
        {
            double winRatio = 0;
            int count = 0;
            foreach(var game in teamSeasonGames)
            {
                if (count == numberOfGames)
                    break;
                if (isWin(game, teamId))
                    winRatio++;
                count++;
            }
            if (count > 0)
                winRatio = winRatio / count;
            return winRatio;
        }

        private static bool isWin(Game game, int teamId)
        {
            if(game.homeTeamId == teamId && game.winner == 0) return true;
            if(game.awayTeamId == teamId && game.winner == 1) return true;
            return false;
        }

        public static double GetGoalsAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double goalsAvg = 0;
            int count = 0;
            foreach(var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamId == teamId)
                    goalsAvg += game.homeGoals;
                else if (game.awayTeamId == teamId)
                    goalsAvg += game.awayGoals;

                count++;
            }
            if (count > 0)
                goalsAvg = goalsAvg / count;
            return goalsAvg;
        }
        public static double GetConcededGoalsAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double goalsAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.awayTeamId == teamId)
                    goalsAvg += game.homeGoals;
                else if (game.homeTeamId == teamId)
                    goalsAvg += game.awayGoals;

                count++;
            }
            if (count > 0)
                goalsAvg = goalsAvg / count;
            return goalsAvg;
        }

        public static double GetSogAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double sogAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamId == teamId)
                    sogAvg += game.homeSOG;
                else if (game.awayTeamId == teamId)
                    sogAvg += game.awaySOG;

                count++;
            }
            if (count > 0)
                sogAvg = sogAvg / count;
            return sogAvg;
        }

        public static double GetBlockedShotsAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double blockedSogAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamId == teamId)
                    blockedSogAvg += game.homeBlockedShots;
                else if (game.awayTeamId == teamId)
                    blockedSogAvg += game.awayBlockedShots;

                count++;
            }
            if (count > 0)
                blockedSogAvg = blockedSogAvg / count;
            return blockedSogAvg;
        }
        public static double GetPpgAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double ppgAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamId == teamId)
                    ppgAvg += game.homePPG;
                else if (game.awayTeamId == teamId)
                    ppgAvg += game.awayPPG;

                count++;
            }
            if (count > 0)
                ppgAvg = ppgAvg / count;
            return ppgAvg;
        }
        public static double GetHitsAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double hitsAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamId == teamId)
                    hitsAvg += game.homeHits;
                else if (game.awayTeamId == teamId)
                    hitsAvg += game.awayHits;

                count++;
            }
            if (count > 0)
                hitsAvg = hitsAvg / count;
            return hitsAvg;
        }
        public static double GetPimAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double pimAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamId == teamId)
                    pimAvg += game.homePIM;
                else if (game.awayTeamId == teamId)
                    pimAvg += game.awayPIM;

                count++;
            }
            if (count > 0)
                pimAvg = pimAvg / count;
            return pimAvg;
        }
        public static double GetTakeawaysAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double takeawayAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamId == teamId)
                    takeawayAvg += game.homeTakeaways;
                else if (game.awayTeamId == teamId)
                    takeawayAvg += game.awayTakeaways;

                count++;
            }
            if (count > 0)
                takeawayAvg = takeawayAvg / count;
            return takeawayAvg;
        }

        public static bool GetIsExcluded(List<Game> awayGames, List<Game> homeGames, int numberOfGamesToExclude)
        {
            if(awayGames.Count() < numberOfGamesToExclude || homeGames.Count() < numberOfGamesToExclude)
                return true;
            return false;
        }

        public static double GetGiveawaysAvgOfRecentGames(List<Game> teamGames, int teamId, int numberOfGames)
        {
            double giveawayAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamId == teamId)
                    giveawayAvg += game.homeGiveaways;
                else if (game.awayTeamId == teamId)
                    giveawayAvg += game.awayGiveaways;

                count++;
            }
            if (count > 0)
                giveawayAvg = giveawayAvg / count;
            return giveawayAvg;
        }
        public static double GetConcededGoalsAvgOfRecentGamesAtHome(List<Game> teamGames, int teamId, int numberOfGames)
        {
            teamGames = teamGames.Where(game => game.homeTeamId == teamId).ToList();
            return GetConcededGoalsAvgOfRecentGames(teamGames, teamId, numberOfGames);
        }
        public static double GetConcededGoalsAvgOfRecentGamesAtAway(List<Game> teamGames, int teamId, int numberOfGames)
        {
            teamGames = teamGames.Where(game => game.awayTeamId == teamId).ToList();
            return GetConcededGoalsAvgOfRecentGames(teamGames, teamId, numberOfGames);
        }
        public static double GetGoalsAvgOfRecentGamesAtHome(List<Game> teamGames, int teamId, int numberOfGames)
        {
            teamGames = teamGames.Where(game => game.homeTeamId == teamId).ToList();
            return GetGoalsAvgOfRecentGames(teamGames, teamId, numberOfGames);
        }
        public static double GetGoalsAvgOfRecentGamesAtAway(List<Game> teamGames, int teamId, int numberOfGames)
        {
            teamGames = teamGames.Where(game => game.awayTeamId == teamId).ToList();
            return GetGoalsAvgOfRecentGames(teamGames, teamId, numberOfGames);
        }
        // If only one game has been played return default rest, otherwise find how many hours
        // since last game
        public static double GetHoursBetweenLastTwoGames(List<Game> games)
        {
            if (games.Count() < 2)
                return DEFAULT_DAYS;
            var currentDate = games[0].gameDate;
            var lastDate = games[1].gameDate;

            var hourDifference = (currentDate - lastDate).TotalHours;

            return hourDifference;
        }
    }
}
