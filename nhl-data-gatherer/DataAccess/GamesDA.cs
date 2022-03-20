using nhl_data_builder.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace nhl_data_gatherer.DataAccess
{
    public class GamesDA
	{
		private string _connectionString;
		public GamesDA(IConfiguration iconfiguration)
		{
			_connectionString = iconfiguration.GetConnectionString("Default");  
		}
		public void AddGames(List<Game> games)
		{
			using (SqlConnection con = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("InsertGame", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				foreach(var game in games)
				{
					cmd.Parameters.AddWithValue("@id",game.id.ToString());
					cmd.Parameters.AddWithValue("@homeTeamName",game.homeTeamName);
					cmd.Parameters.AddWithValue("@awayTeamName",game.awayTeamName);
					cmd.Parameters.AddWithValue("@seasonStartYear",game.seasonStartYear.ToString());
					cmd.Parameters.AddWithValue("@gameDate",game.gameDate.ToString());
					cmd.Parameters.AddWithValue("@homeGoals",game.homeGoals.ToString());
					cmd.Parameters.AddWithValue("@awayGoals",game.awayGoals.ToString());
					cmd.Parameters.AddWithValue("@winner",game.winner.ToString());
					cmd.Parameters.AddWithValue("@homeSOG",game.homeSOG.ToString());
					cmd.Parameters.AddWithValue("@awaySOG",game.awaySOG.ToString());
					cmd.Parameters.AddWithValue("@homePPG",game.homePPG.ToString());
					cmd.Parameters.AddWithValue("@awayPPG",game.awayPPG.ToString());
					cmd.Parameters.AddWithValue("@homePIM",game.homePIM.ToString());
					cmd.Parameters.AddWithValue("@awayPIM",game.awayPIM.ToString());
					cmd.Parameters.AddWithValue("@homeFaceOffWinPercent",game.homeFaceOffWinPercent.ToString());
					cmd.Parameters.AddWithValue("@awayFaceOffWinPercent",game.awayFaceOffWinPercent.ToString());
					cmd.Parameters.AddWithValue("@homeBlockedShots",game.homeBlockedShots.ToString());
					cmd.Parameters.AddWithValue("@awayBlockedShots",game.awayBlockedShots.ToString());
					cmd.Parameters.AddWithValue("@homeHits",game.homeHits.ToString());
					cmd.Parameters.AddWithValue("@awayHits",game.awayHits.ToString());
					cmd.Parameters.AddWithValue("@homeTakeaways",game.homeTakeaways.ToString());
					cmd.Parameters.AddWithValue("@awayTakeaways",game.awayTakeaways.ToString());
					cmd.Parameters.AddWithValue("@homeGiveaways",game.homeGiveaways.ToString());
					cmd.Parameters.AddWithValue("@awayGiveaways",game.awayGiveaways.ToString());

					cmd.ExecuteNonQuery(); // Execute Query
					cmd.Parameters.Clear();
				}
			}
		}
		public List<Game> GetGames()
		{
			var games = new List<Game>();  
			var table = new DataTable();    
			using (var da = new SqlDataAdapter("SELECT * FROM Game", _connectionString))
			{      
				da.Fill(table);
			}
			foreach(DataRow row in table.Rows)
			{
				games.Add(new Game(){
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
					homePPG = Convert.ToDouble(row["homePPG"]),
					awayPPG = Convert.ToDouble(row["awayPPG"]),
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
				});
			}
			return games;
		}
	}
}
