namespace Entities.Models
{
    public class FutureGame
    {
        public int id { get; set; }
        public string homeTeamName { get; set; } = string.Empty;
        public string awayTeamName { get; set; } = string.Empty;
        public DateTime gameDate { get; set; }
        public bool IsValid()
        {
            return homeTeamName != null && homeTeamName != string.Empty;
        }
    }
}
