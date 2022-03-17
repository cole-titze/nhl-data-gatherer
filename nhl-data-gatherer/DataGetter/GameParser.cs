using Newtonsoft.Json;
using nhl_data_builder.Entities;

namespace nhl_data_builder.DataGetter
{
    public class GameParser : IGameParser
	{
        public async Task<Game> BuildGame(HttpResponseMessage response)
        {
            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);
            if (InvalidGame(message))
                return new Game();

            var game = ParseMessageToGame(message);
            return game;
        }

        private Game ParseMessageToGame(dynamic message)
        {
            var homeTeam = message.liveData.boxscore.teams.home.teamStats.teamSkaterStats;
            var awayTeam = message.liveData.boxscore.teams.away.teamStats.teamSkaterStats;

            var game = new Game(){
                id = (int)message.gamePk,
                homeGoals = (int)homeTeam.goals,
                awayGoals = (int)awayTeam.goals,
                homeTeamName = (string)message.gameData.teams.home.name,
                awayTeamName = (string)message.gameData.teams.away.name,
                homeSOG = (int)homeTeam.shots,
                awaySOG = (int)awayTeam.shots,
                homePPG = (int)homeTeam.powerPlayGoals,
                awayPPG = (int)awayTeam.powerPlayGoals,
                homePIM = (int)homeTeam.pim,
                awayPIM = (int)awayTeam.pim,
                homeFaceOffWinPercent = (double)homeTeam.faceOffWinPercentage,
                awayFaceOffWinPercent = (double)awayTeam.faceOffWinPercentage,
                homeBlockedShots = (int)homeTeam.blocked,
                awayBlockedShots = (int)awayTeam.blocked,
                homeHits = (int)homeTeam.hits,
                awayHits = (int)awayTeam.hits,
                homeTakeaways = (int)homeTeam.takeaways,
                awayTakeaways = (int)awayTeam.takeaways,
                homeGiveaways = (int)homeTeam.giveaways,
                awayGiveaways = (int)awayTeam.giveaways,
                winner = (int)GetWinner((int)homeTeam.goals, (int)awayTeam.goals),
                seasonStartYear = GetSeason((string)message.gameData.game.season),
                gameDate = GetDate((string)message.gameData.datetime.dateTime),
            };
            return game;
        }

        private DateTime GetDate(string dateTime)
        {
            return DateTime.Parse(dateTime);
        }

        private int GetSeason(string season)
        {
            var yearStr = season.Substring(0,4);
            return int.Parse(yearStr);
        }
        
        private Winner GetWinner(int homeGoals, int awayGoals)
        {
            if( homeGoals > awayGoals )
                return Winner.HOME;
            return Winner.AWAY;
        }

        private bool InvalidGame(dynamic message)
        {
            if(message == null)
                return true;

            float homeFaceoffs = (float)message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.faceOffWinPercentage;
            float awayFaceoffs = (float)message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.faceOffWinPercentage;
            return (homeFaceoffs == 0 && awayFaceoffs == 0);
        }
	}
}