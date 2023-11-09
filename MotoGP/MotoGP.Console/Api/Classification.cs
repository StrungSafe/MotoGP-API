using System.Text.Json.Serialization;

namespace MotoGP.Api
{
    public class Classification
    {
        [JsonPropertyName("average_speed")]
        public decimal AverageSpeed { get; set; }

        [JsonPropertyName("best_lap")]
        public BestLap BestLap { get; set; }

        public Constructor Constructor { get; set; }

        public Gap Gap { get; set; }

        public Guid Id { get; set; }

        public int Points { get; set; }

        public int? Position { get; set; }

        public Rider Rider { get; set; }

        public string Status { get; set; }

        public Team Team { get; set; }

        public string Time { get; set; }

        [JsonPropertyName("top_speed")]
        public decimal TopSpeed { get; set; }

        [JsonPropertyName("total_laps")]
        public int TotalLaps { get; set; }
    }
}