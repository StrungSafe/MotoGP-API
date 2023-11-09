using MotoGP.Api;

namespace MotoGP.Extensions
{
    public static class InterfaceExtensions
    {
        public static Category GetMotoGp(this List<Category> categories)
        {
            return categories.First(c => c.Name.StartsWith("MotoGP", StringComparison.CurrentCultureIgnoreCase));
        }

        public static string[] GetMotoGpRacers(this Event _event)
        {
            return _event.Categories.GetMotoGp().GetRacers();
        }

        public static string GetMotoGpWinner(this Event _event)
        {
            Category motoGp = _event.Categories.GetMotoGp();
            Session race = motoGp.GetRace();
            Classification winner = race.SessionClassification.Classifications.First(c => c.Position == 1);
            return winner.Rider.FullName;
        }

        public static Session GetRace(this Category category)
        {
            return category.Sessions.First(s =>
                string.Equals(s.Type, "RAC", StringComparison.CurrentCultureIgnoreCase));
        }

        public static string[] GetRacers(this Category category)
        {
            var races = new Dictionary<string, string>();
            category.Sessions.ForEach(s =>
                s.SessionClassification.Classifications.ForEach(c => races.TryAdd(c.Rider.FullName, c.Rider.FullName)));
            return races.Keys.ToArray();
        }

        public static bool HasMotoGpRace(this List<Category> categories)
        {
            bool motoGpCat =
                categories.Exists(c => c.Name.StartsWith("MotoGP", StringComparison.CurrentCultureIgnoreCase));
            return motoGpCat && categories
                                .First(c => c.Name.StartsWith("MotoGP", StringComparison.CurrentCultureIgnoreCase))
                                .Sessions.Exists(s =>
                                    string.Equals(s.Type, "RAC", StringComparison.CurrentCultureIgnoreCase));
        }

        public static bool HasMotoGpWinner(this Event _event)
        {
            return _event.Categories.HasMotoGpRace() && _event.Categories.GetMotoGp().GetRace().SessionClassification
                                                              .Classifications.Exists(c => c.Position == 1);
        }
    }
}