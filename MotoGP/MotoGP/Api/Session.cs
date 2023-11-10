namespace MotoGP.Api
{
    public class Session
    {
        public string Circuit { get; set; }

        public Condition Condition { get; set; }

        public Guid Id { get; set; }

        public SessionClassification SessionClassification { get; set; }

        public string Type { get; set; }
    }
}