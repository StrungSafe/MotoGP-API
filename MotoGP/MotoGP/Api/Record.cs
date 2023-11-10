namespace MotoGP.Api
{
    public class Record
    {
        public BestLap BestLap { get; set; }

        public bool IsNewRecord { get; set; }

        public Rider Rider { get; set; }

        public decimal Speed { get; set; }

        public string Type { get; set; }
    }
}