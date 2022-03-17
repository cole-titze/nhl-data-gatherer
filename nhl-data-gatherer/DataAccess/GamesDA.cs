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
				SqlCommand cmd = new SqlCommand("GET_ALL_Games", con);
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
	}
}
