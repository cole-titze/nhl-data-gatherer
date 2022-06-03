namespace Entities.Models
{
    public class PlayerValue
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public double value { get; set; }
        public int startYear { get; set; }
        public string position { get; set; } = string.Empty;
    }
}