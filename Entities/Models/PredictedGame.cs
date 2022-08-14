namespace Entities.Models
{
	public class PredictedGame
	{
        public int id { get; set; }
        public int homeTeamId { get; set; }
        public int awayTeamId { get; set; }
        public DateTime gameDate { get; set; }
        public double vegasHomeOdds { get; set; }
        public double vegasAwayOdds { get; set; }
        public double modelHomeOdds { get; set; }
        public double modelAwayOdds { get; set; }
        public bool hasBeenPlayed { get; set; }
    }
}

