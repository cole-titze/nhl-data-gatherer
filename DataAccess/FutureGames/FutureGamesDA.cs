using Entities.Models;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.FutureGames
{
    public class FutureGamesDA : IFutureGamesDA
    {
        private string _connectionString;
        private List<int> _futureGameIds = new List<int>();
        public FutureGamesDA(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddFutureGames(List<FutureGame> games)
        {
            var gameTable = new DataTable();
            CacheFutureGameIds();
            using (var da = new SqlDataAdapter("SELECT * FROM FutureGame WHERE 0 = 1", _connectionString))
            {
                da.Fill(gameTable);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.UpdateCommand = cb.GetUpdateCommand();

                foreach (var game in games)
                {
                    // Skip if game exists
                    if (_futureGameIds.Contains(game.id))
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
        public void CacheFutureGameIds()
        {
            _futureGameIds.Clear();
            var dataTable = new DataTable();
            using (var da = new SqlDataAdapter($"SELECT id FROM FutureGame", _connectionString))
            {
                da.Fill(dataTable);
            }
            foreach (DataRow row in dataTable.Rows)
            {
                _futureGameIds.Add(Convert.ToInt32(row["id"]));
            }
        }

    }
}
