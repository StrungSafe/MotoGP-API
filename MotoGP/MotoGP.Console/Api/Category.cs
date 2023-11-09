namespace MotoGP.Api
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}