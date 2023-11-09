using MotoGP.Api;

namespace MotoGP.Data
{
    public interface IDataLoader
    {
        Task<IEnumerable<Season>> Load(CancellationToken token = default);
    }
}