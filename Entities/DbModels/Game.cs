namespace nhl_data_builder.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string homeTeam { get; set; } = string.Empty;
        public string awayTeam { get; set; } = string.Empty;
        public DateTime season { get; set; }
        public DateTime date { get; set; }
        public int homeGoals { get; set; }
        public int awayGoals { get; set; }
        public int winner { get; set; }
        public int homeSOG { get; set; }
        public int awaySOG { get; set; }
        public double homePPG { get; set; }
        public double awayPPg { get; set; }
        public int homePIM { get; set; }
        public int awayPIM { get; set; }
        public double homeFaceOffWinPercent { get; set; }
        public double awayFaceOffWinPercent { get; set; }
        public int homeBlockedShots { get; set; }
        public int awayBlockedShots { get; set; }
        public int homeHits { get; set; }
        public int awayHits { get; set; }
        public int homeTakeaways { get; set; }
        public int awayTakeaways { get; set; }
        public int homeGiveaways { get; set; }
        public int awayGiveaways { get; set; }
    }
}