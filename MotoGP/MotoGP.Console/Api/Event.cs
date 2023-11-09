namespace MotoGP.Api
{
    public class Event
    {
        public List<Category> Categories { get; set; } = new List<Category>();

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Test { get; set; }
    }
}