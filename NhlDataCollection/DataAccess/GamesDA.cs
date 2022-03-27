using Entities.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NhlDataCollection.DataAccess
{
    public class GamesDA : IGamesDA
    {
        private string _connectionString;
        public GamesDA(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddGames(List<Game> games)
        {
            var gameTable = new DataTable();

            using (var da = new SqlDataAdapter("SELECT * FROM Game WHERE 0 = 1", _connectionString))
            {
                da.Fill(gameTable);
                foreach (var game in games)
                {
                    var newRow = gameTable.NewRow();
                    newRow["id"] = game.id;
                    newRow["homeTeamName"] = game.homeTeamName;
                    newRow["awayTeamName"] = game.awayTeamName;
                    newRow["seasonStartYear"] = game.seasonStartYear;
                    newRow["gameDate"] = game.gameDate;
                    newRow["homeGoals"] = game.homeGoals;
                    newRow["awayGoals"] = game.awayGoals;
                    newRow["winner"] = game.winner;
                    newRow["homeSOG"] = game.homeSOG;
                    newRow["awaySOG"] = game.awaySOG;
                    newRow["homePPG"] = game.homePPG;
                    newRow["awayPPG"] = game.awayPPG;
                    newRow["homePIM"] = game.homePIM;
                    newRow["awayPIM"] = game.awayPIM;
                    newRow["homeFaceOffWinPercent"] = game.homeFaceOffWinPercent;
                    newRow["awayFaceOffWinPercent"] = game.awayFaceOffWinPercent;
                    newRow["homeBlockedShots"] = game.homeBlockedShots;
                    newRow["awayBlockedShots"] = game.awayBlockedShots;
                    newRow["homeHits"] = game.homeHits;
                    newRow["awayHits"] = game.awayHits;
                    newRow["homeTakeaways"] = game.homeTakeaways;
                    newRow["awayTakeaways"] = game.awayTakeaways;
                    newRow["homeGiveaways"] = game.homeGiveaways;
                    newRow["awayGiveaways"] = game.awayGiveaways;

                    gameTable.Rows.Add(newRow);
                }
                new SqlCommandBuilder(da);
                da.Update(gameTable);
            }
        }

        public Game GetGameById(int id)
        {
            var gameTable = new DataTable();
            using (var da = new SqlDataAdapter($"SELECT * FROM Game WHERE id = {id}", _connectionString))
            {
                da.Fill(gameTable);
            }
            if (gameTable.Rows.Count == 0)
                return new Game();
            return MapDataRowToGame(gameTable.Rows[0]);
        }

        public int GetGameCountBySeason(int year)
        {
            int count;
            string sql = $"SELECT COUNT(id) FROM Game WHERE seasonStartYear = {year};";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            return count;
        }

        public List<Game> GetGames()
        {
            var games = new List<Game>();
            var table = new DataTable();
            using (var da = new SqlDataAdapter("SELECT * FROM Game", _connectionString))
            {
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                games.Add(MapDataRowToGame(row));
            }
            return games;
        }

        public Game MapDataRowToGame(DataRow row)
        {
            return new Game()
            {
                id = Convert.ToInt32(row["id"]),
                homeTeamName = row["homeTeamName"].ToString(),
                awayTeamName = row["awayTeamName"].ToString(),
                seasonStartYear = Convert.ToInt32(row["seasonStartYear"]),
                gameDate = Convert.ToDateTime(row["gameDate"]),
                homeGoals = Convert.ToInt32(row["homeGoals"]),
                awayGoals = Convert.ToInt32(row["awayGoals"]),
                winner = Convert.ToInt32(row["winner"]),
                homeSOG = Convert.ToInt32(row["homeSOG"]),
                awaySOG = Convert.ToInt32(row["awaySOG"]),
                homePPG = Convert.ToInt32(row["homePPG"]),
                awayPPG = Convert.ToInt32(row["awayPPG"]),
                homePIM = Convert.ToInt32(row["homePIM"]),
                awayPIM = Convert.ToInt32(row["awayPIM"]),
                homeFaceOffWinPercent = Convert.ToDouble(row["homeFaceOffWinPercent"]),
                awayFaceOffWinPercent = Convert.ToDouble(row["awayFaceOffWinPercent"]),
                homeBlockedShots = Convert.ToInt32(row["homeBlockedShots"]),
                awayBlockedShots = Convert.ToInt32(row["awayBlockedShots"]),
                homeHits = Convert.ToInt32(row["homeHits"]),
                awayHits = Convert.ToInt32(row["awayHits"]),
                homeGiveaways = Convert.ToInt32(row["homeGiveaways"]),
                awayGiveaways = Convert.ToInt32(row["awayGiveaways"]),
                homeTakeaways = Convert.ToInt32(row["homeTakeaways"]),
                awayTakeaways = Convert.ToInt32(row["awayTakeaways"]),
            };
        }
    }
}
