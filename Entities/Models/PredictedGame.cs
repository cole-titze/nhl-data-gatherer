namespace Entities.Models
{
	public class PredictedGame
	{
        public int id { get; set; }
        public int homeTeamId { get; set; }
        public int awayTeamId { get; set; }
        public DateTime gameDate { get; set; }
        public double bovadaVegasHomeOdds { get; set; }
        public double bovadaVegasAwayOdds { get; set; }
        public double myBookieVegasHomeOdds { get; set; }
        public double myBookieVegasAwayOdds { get; set; }
        public double pinnacleVegasHomeOdds { get; set; }
        public double pinnacleVegasAwayOdds { get; set; }
        public double betOnlineVegasHomeOdds { get; set; }
        public double betOnlineVegasAwayOdds { get; set; }
        public double bet365VegasHomeOdds { get; set; }
        public double bet365VegasAwayOdds { get; set; }
        public double modelHomeOdds { get; set; }
        public double modelAwayOdds { get; set; }
    }
}

