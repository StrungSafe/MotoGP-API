using MotoGP.Api;

namespace MotoGP.Repositories;

public interface IDataRepository
{
    Task<Category[]> GetCategories(Guid seasonId, Guid eventId, CancellationToken token = default);

    Task<Event[]> GetEvents(Guid seasonId, bool isFinished, CancellationToken token = default);

    Task<Season[]> GetSeasons(CancellationToken token = default);

    Task<SessionClassification> GetSessionClassification(Guid seasonId, Guid eventId, Guid categoryId, Guid sessionId,
        bool test, CancellationToken token = default);

    Task<Session[]> GetSessions(Guid seasonId, Guid eventId, Guid categoryId, CancellationToken token = default);
}