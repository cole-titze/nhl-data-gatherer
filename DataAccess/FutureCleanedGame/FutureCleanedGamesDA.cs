﻿using Entities.Models;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.FutureCleanedGame
{
    public class FutureCleanedGamesDA : IFutureCleanedGamesDA
    {
        private string _connectionString;
        private List<int> _cachedGameIds;
        public FutureCleanedGamesDA(string connectionString)
        {
            _connectionString = connectionString;
            _cachedGameIds = new List<int>();
        }

        public void AddFutureGames(List<CleanedGame> games)
        {
            var gameTable = new DataTable();
            CacheFutureGameIds();

            using (var da = new SqlDataAdapter("SELECT * FROM FutureCleanedGame WHERE 0 = 1", _connectionString))
            {
                da.Fill(gameTable);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.UpdateCommand = cb.GetUpdateCommand();

                foreach (var game in games)
                {
                    if (_cachedGameIds.Contains(game.id))
                        continue;
                    var newRow = gameTable.NewRow();
                    newRow["id"] = game.id;
                    newRow["homeTeamName"] = game.homeTeamName;
                    newRow["awayTeamName"] = game.awayTeamName;
                    newRow["seasonStartYear"] = game.seasonStartYear;
                    newRow["gameDate"] = game.gameDate;
                    newRow["homeWinRatio"] = game.homeWinRatio;
                    newRow["homeRecentWinRatio"] = game.homeRecentWinRatio;
                    newRow["homeRecentGoalsAvg"] = game.homeRecentGoalsAvg;
                    newRow["homeRecentConcededGoalsAvg"] = game.homeRecentConcededGoalsAvg;
                    newRow["homeRecentSogAvg"] = game.homeRecentSogAvg;
                    newRow["homeRecentPpgAvg"] = game.homeRecentPpgAvg;
                    newRow["homeRecentHitsAvg"] = game.homeRecentHitsAvg;
                    newRow["homeRecentPimAvg"] = game.homeRecentPimAvg;
                    newRow["homeRecentBlockedShotsAvg"] = game.homeRecentBlockedShotsAvg;
                    newRow["homeRecentTakeawaysAvg"] = game.homeRecentTakeawaysAvg;
                    newRow["homeRecentGiveawaysAvg"] = game.homeRecentGiveawaysAvg;
                    newRow["homeGoalsAvg"] = game.homeGoalsAvg;
                    newRow["homeGoalsAvgAtHome"] = game.homeGoalsAvgAtHome;
                    newRow["homeRecentGoalsAvgAtHome"] = game.homeRecentGoalsAvgAtHome;
                    newRow["homeConcededGoalsAvg"] = game.homeConcededGoalsAvg;
                    newRow["homeConcededGoalsAvgAtHome"] = game.homeConcededGoalsAvgAtHome;
                    newRow["homeRecentConcededGoalsAvgAtHome"] = game.homeRecentConcededGoalsAvgAtHome;
                    newRow["awayWinRatio"] = game.awayWinRatio;
                    newRow["awayRecentWinRatio"] = game.awayRecentWinRatio;
                    newRow["awayRecentGoalsAvg"] = game.awayRecentGoalsAvg;
                    newRow["awayRecentConcededGoalsAvg"] = game.awayRecentConcededGoalsAvg;
                    newRow["awayRecentSogAvg"] = game.awayRecentSogAvg;
                    newRow["awayRecentPpgAvg"] = game.awayRecentPpgAvg;
                    newRow["awayRecentHitsAvg"] = game.awayRecentHitsAvg;
                    newRow["awayRecentPimAvg"] = game.awayRecentPimAvg;
                    newRow["awayRecentBlockedShotsAvg"] = game.awayRecentBlockedShotsAvg;
                    newRow["awayRecentTakeawaysAvg"] = game.awayRecentTakeawaysAvg;
                    newRow["awayRecentGiveawaysAvg"] = game.awayRecentGiveawaysAvg;
                    newRow["awayGoalsAvg"] = game.awayGoalsAvg;
                    newRow["awayGoalsAvgAtAway"] = game.awayGoalsAvgAtAway;
                    newRow["awayRecentGoalsAvgAtAway"] = game.awayRecentGoalsAvgAtAway;
                    newRow["awayConcededGoalsAvg"] = game.awayConcededGoalsAvg;
                    newRow["awayConcededGoalsAvgAtAway"] = game.awayConcededGoalsAvgAtAway;
                    newRow["awayRecentConcededGoalsAvgAtAway"] = game.awayRecentConcededGoalsAvgAtAway;
                    gameTable.Rows.Add(newRow);
                }
                da.Update(gameTable);
            }
        }
        private void CacheFutureGameIds()
        {
            _cachedGameIds.Clear();
            var dataTable = new DataTable();
            using (var da = new SqlDataAdapter($"SELECT id FROM FutureCleanedGame", _connectionString))
            {
                da.Fill(dataTable);
            }
            foreach (DataRow row in dataTable.Rows)
            {
                _cachedGameIds.Add(Convert.ToInt32(row["id"]));
            }
        }
    }
}
