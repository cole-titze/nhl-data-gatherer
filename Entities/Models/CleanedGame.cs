using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    // Recent means within last few games ex. 5 games (decided in appsettings)
    public class CleanedGame
    {
        public int id { get; set; }
        public string homeTeamName { get; set; } = string.Empty;
        public string awayTeamName { get; set; } = string.Empty;
        public int seasonStartYear { get; set; }
        public double homeWinRatio { get; set; }
        public double homeRecentWinRatio { get; set; }
        public double homeRecentGoalsAvg { get; set; }
        public double homeRecentConcededGoalsAvg { get; set; }
        public double homeRecentSogAvg { get; set; }
        public double homeRecentPpgAvg { get; set; }
        public double homeRecentHitsAvg { get; set; }
        public double homeRecentPimAvg { get; set; }
        public double homeRecentBlockedShots { get; set; }
        public double homeRecentTakeawaysAvg { get; set; }
        public double homeRecentGiveawaysAvg { get; set; }
        public double homeGoalsAvg { get; set; }
        public double homeGoalsAvgAtHome { get; set; }
        public double homeConcededGoalsAvg { get; set; }
        public double homeConcededGoalsAvgAtHome { get; set; }
        public double awayWinRatio { get; set; }
        public double awayRecentWinRatio { get; set; }
        public double awayRecentGoalsAvg { get; set; }
        public double awayRecentConcededGoals { get; set; }
        public double awayRecentSogAvg { get; set; }
        public double awayRecentPpgAvg { get; set; }
        public double awayRecentHitsAvg { get; set; }
        public double awayRecentPimAvg { get; set; }
        public double awayRecentBlockedShots { get; set; }
        public double awayRecentTakeawaysAvg { get; set; }
        public double awayRecentGiveawaysAvg { get; set; }
        public double awayGoalsAvg { get; set; }
        public double awayGoalsAvgAtAway { get; set; }
        public double awayConcededGoalsAvg { get; set; }
        public double awayConcededGoalsAvgAtAway { get; set; }
    }
}
