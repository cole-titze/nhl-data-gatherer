using Entities.Models;

namespace NhlDataCleaning.Mappers
{
    public static class Cleaner
    {
        public static double GetWinRatioOfRecentGames(List<Game> teamSeasonGames, string teamName, int numberOfGames)
        {
            double winRatio = 0;
            int count = 0;
            foreach(var game in teamSeasonGames)
            {
                if (count == numberOfGames)
                    break;
                if (isWin(game, teamName))
                    winRatio++;
                count++;
            }
            if (count > 0)
                winRatio = winRatio / count;
            return winRatio;
        }

        private static bool isWin(Game game, string teamName)
        {
            if(game.homeTeamName == teamName && game.winner == 0) return true;
            if(game.awayTeamName == teamName && game.winner == 1) return true;
            return false;
        }

        public static double GetGoalsAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double goalsAvg = 0;
            int count = 0;
            foreach(var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamName == teamName)
                    goalsAvg += game.homeGoals;
                else if (game.awayTeamName == teamName)
                    goalsAvg += game.awayGoals;

                count++;
            }
            if (count > 0)
                goalsAvg = goalsAvg / count;
            return goalsAvg;
        }
        public static double GetConcededGoalsAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double goalsAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.awayTeamName == teamName)
                    goalsAvg += game.homeGoals;
                else if (game.homeTeamName == teamName)
                    goalsAvg += game.awayGoals;

                count++;
            }
            if (count > 0)
                goalsAvg = goalsAvg / count;
            return goalsAvg;
        }

        public static double GetSogAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double sogAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamName == teamName)
                    sogAvg += game.homeSOG;
                else if (game.awayTeamName == teamName)
                    sogAvg += game.awaySOG;

                count++;
            }
            if (count > 0)
                sogAvg = sogAvg / count;
            return sogAvg;
        }

        public static double GetBlockedShotsAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double blockedSogAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamName == teamName)
                    blockedSogAvg += game.homeBlockedShots;
                else if (game.awayTeamName == teamName)
                    blockedSogAvg += game.awayBlockedShots;

                count++;
            }
            if (count > 0)
                blockedSogAvg = blockedSogAvg / count;
            return blockedSogAvg;
        }
        public static double GetPpgAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double ppgAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamName == teamName)
                    ppgAvg += game.homePPG;
                else if (game.awayTeamName == teamName)
                    ppgAvg += game.awayPPG;

                count++;
            }
            if (count > 0)
                ppgAvg = ppgAvg / count;
            return ppgAvg;
        }
        public static double GetHitsAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double hitsAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamName == teamName)
                    hitsAvg += game.homeHits;
                else if (game.awayTeamName == teamName)
                    hitsAvg += game.awayHits;

                count++;
            }
            if (count > 0)
                hitsAvg = hitsAvg / count;
            return hitsAvg;
        }
        public static double GetPimAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double pimAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamName == teamName)
                    pimAvg += game.homePIM;
                else if (game.awayTeamName == teamName)
                    pimAvg += game.awayPIM;

                count++;
            }
            if (count > 0)
                pimAvg = pimAvg / count;
            return pimAvg;
        }
        public static double GetTakeawaysAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double takeawayAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamName == teamName)
                    takeawayAvg += game.homeTakeaways;
                else if (game.awayTeamName == teamName)
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

        public static double GetGiveawaysAvgOfRecentGames(List<Game> teamGames, string teamName, int numberOfGames)
        {
            double giveawayAvg = 0;
            int count = 0;
            foreach (var game in teamGames)
            {
                if (count == numberOfGames)
                    break;
                if (game.homeTeamName == teamName)
                    giveawayAvg += game.homeGiveaways;
                else if (game.awayTeamName == teamName)
                    giveawayAvg += game.awayGiveaways;

                count++;
            }
            if (count > 0)
                giveawayAvg = giveawayAvg / count;
            return giveawayAvg;
        }
        public static double GetConcededGoalsAvgOfRecentGamesAtHome(List<Game> teamGames, string teamName, int numberOfGames)
        {
            teamGames = teamGames.Where(game => game.homeTeamName == teamName).ToList();
            return GetConcededGoalsAvgOfRecentGames(teamGames, teamName, numberOfGames);
        }
        public static double GetConcededGoalsAvgOfRecentGamesAtAway(List<Game> teamGames, string teamName, int numberOfGames)
        {
            teamGames = teamGames.Where(game => game.awayTeamName == teamName).ToList();
            return GetConcededGoalsAvgOfRecentGames(teamGames, teamName, numberOfGames);
        }
        public static double GetGoalsAvgOfRecentGamesAtHome(List<Game> teamGames, string teamName, int numberOfGames)
        {
            teamGames = teamGames.Where(game => game.homeTeamName == teamName).ToList();
            return GetGoalsAvgOfRecentGames(teamGames, teamName, numberOfGames);
        }
        public static double GetGoalsAvgOfRecentGamesAtAway(List<Game> teamGames, string teamName, int numberOfGames)
        {
            teamGames = teamGames.Where(game => game.awayTeamName == teamName).ToList();
            return GetGoalsAvgOfRecentGames(teamGames, teamName, numberOfGames);
        }
    }
}
