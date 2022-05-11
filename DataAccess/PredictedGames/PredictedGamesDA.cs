using Entities.Models;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.PredictedGames
{
    public class PredictedGamesDA : IPredictedGamesDA
    {
        private string _connectionString;
        private List<int> _predictedGameIds = new List<int>();

        public PredictedGamesDA(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddPredictedGames(List<FutureGame> games)
        {
            CachePredictedGameIds();

            var gameTable = new DataTable();
            using (var da = new SqlDataAdapter("SELECT * FROM PredictedGame WHERE 0 = 1", _connectionString))
            {
                da.Fill(gameTable);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.UpdateCommand = cb.GetUpdateCommand();

                foreach (var game in games)
                {
                    if (_predictedGameIds.Contains(game.id))
                        continue;
                    var newRow = gameTable.NewRow();
                    newRow["id"] = game.id;
                    newRow["homeTeamName"] = game.homeTeamName;
                    newRow["awayTeamName"] = game.awayTeamName;
                    newRow["gameDate"] = game.gameDate;
                    gameTable.Rows.Add(newRow);
                }
                da.Update(gameTable);
            }
        }
        private void CachePredictedGameIds()
        {
            _predictedGameIds.Clear();
            var dataTable = new DataTable();
            using (var da = new SqlDataAdapter($"SELECT id FROM PredictedGame", _connectionString))
            {
                da.Fill(dataTable);
            }
            foreach (DataRow row in dataTable.Rows)
            {
                _predictedGameIds.Add(Convert.ToInt32(row["id"]));
            }
        }
    }
}
