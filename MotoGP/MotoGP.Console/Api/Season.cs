namespace MotoGP.Api
{
    public class Season
    {
        public bool Current { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();

        public Guid Id { get; set; }

        public int Year { get; set; }
    }
}