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
		public List<Game> GetGames()  
		{
			var games = new List<Game>();  

			using (SqlConnection con = new SqlConnection(_connectionString))
			{
				SqlCommand cmd = new SqlCommand("GetGames", con);
				cmd.CommandType = CommandType.StoredProcedure;
				con.Open();
				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					games.Add(new Game
					{
						id = Convert.ToInt32(rdr[0]),
						homeTeamName = rdr[1].ToString(),
						awayTeamName = rdr[2].ToString(),
						seasonStartYear = Convert.ToInt32(rdr[3]),
						gameDate = DateTime.Parse(rdr[4].ToString()),
						homeGoals = Convert.ToInt32(rdr[5]),
						awayGoals = Convert.ToInt32(rdr[6]),
						winner = Convert.ToInt32(rdr[7]),
						homeSOG = Convert.ToInt32(rdr[8]),
						awaySOG = Convert.ToInt32(rdr[9]),
						homePPG = Convert.ToInt32(rdr[10]),
						awayPPG = Convert.ToInt32(rdr[11]),
						homePIM = Convert.ToInt32(rdr[12]),
						awayPIM = Convert.ToInt32(rdr[13]),
						homeFaceOffWinPercent = Convert.ToDouble(rdr[14]),
						awayFaceOffWinPercent = Convert.ToDouble(rdr[15]),
						homeBlockedShots = Convert.ToInt32(rdr[16]),
						awayBlockedShots = Convert.ToInt32(rdr[17]),
						homeHits = Convert.ToInt32(rdr[18]),
						awayHits = Convert.ToInt32(rdr[19]),
						homeTakeaways = Convert.ToInt32(rdr[20]),
						awayTakeaways = Convert.ToInt32(rdr[21]),
						homeGiveaways = Convert.ToInt32(rdr[22]),
						awayGiveaways = Convert.ToInt32(rdr[23]),
					});
				}
			}
			return games;
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
	}
}
