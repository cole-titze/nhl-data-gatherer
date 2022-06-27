namespace Entities.Models
{
    public class FutureGame
    {
        public int id { get; set; }
        public int homeTeamId { get; set; }
        public int awayTeamId { get; set; }
        public DateTime gameDate { get; set; }
        public bool IsValid()
        {
            return homeTeamId != 0;
        }
    }
}
