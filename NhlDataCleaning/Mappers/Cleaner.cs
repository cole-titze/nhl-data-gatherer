using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
