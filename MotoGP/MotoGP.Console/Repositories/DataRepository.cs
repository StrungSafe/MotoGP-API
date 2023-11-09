using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotoGP.Api;
using MotoGP.Configuration;
using MotoGP.Utilities;

namespace MotoGP.Repositories;

public class DataRepository : IDataRepository
{
    private readonly MotoGpClient client;

    private readonly ILogger<DataRepository> logger;

    private readonly IDataReader reader;

    private readonly Repository settings;

    private readonly IDataWriter writer;

    public DataRepository(ILogger<DataRepository> logger, MotoGpClient client,
        IOptions<Repository> settings, IDataWriter writer, IDataReader reader)
    {
        this.logger = logger;
        this.client = client;
        this.settings = settings.Value;
        this.writer = writer;
        this.reader = reader;
    }

    public Task<Category[]> GetCategories(Guid seasonId, Guid eventId, CancellationToken token)
    {
        return GetFromJson<Category[]>($"categories?eventUuid={eventId}",
            $"seasons/{seasonId}/{eventId}/categories.json", token);
    }

    public Task<Event[]> GetEvents(Guid seasonId, bool isFinished, CancellationToken token)
    {
        return GetFromJson<Event[]>($"events?seasonUuid={seasonId}&isFinished={isFinished}",
            $"seasons/{seasonId}/events_{isFinished}.json", token);
    }

    public Task<Season[]> GetSeasons(CancellationToken token)
    {
        return GetFromJson<Season[]>("seasons", "seasons/seasons.json", token);
    }

    public Task<SessionClassification> GetSessionClassification(Guid seasonId, Guid eventId, Guid categoryId,
        Guid sessionId, bool test, CancellationToken token)
    {
        return GetFromJson<SessionClassification>(
            $"session/{sessionId}/classification?test={test}",
            $"seasons/{seasonId}/{eventId}/{categoryId}/{sessionId}/classifications_{test}.json", token);
    }

    public Task<Session[]> GetSessions(Guid seasonId, Guid eventId, Guid categoryId, CancellationToken token)
    {
        return GetFromJson<Session[]>(
            $"sessions?eventUuid={eventId}&categoryUuid={categoryId}",
            $"seasons/{seasonId}/{eventId}/{categoryId}/sessions.json", token);
    }

    private async Task<T> FromApi<T>(string relativeUrl, string relativeUri, CancellationToken token)
    {
        try
        {
            var data = await client.GetAsync<T>(new Uri(relativeUrl, UriKind.Relative), token);
            if (data == null)
            {
                //TODO better handling? is this a real scenario
                throw new Exception($"An object was expected but a null object was returned from '{relativeUrl}'");
            }
            await writer.Write(relativeUri, data, token);
            return data;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error thrown trying to get data from API"); //TODO
            throw;
        }
    }

    private async Task<T> GetFromJson<T>(string relativeUrl, string relativeUri, CancellationToken cancellationToken)
    {
        using IDisposable? scope =
            logger.BeginScope("API Url: {relativeUrl} URI: {relativeUri}", relativeUrl, relativeUri);

        bool cacheEnabled = settings.LocalCache.Enabled;
        bool overwriteCache = settings.LocalCache.Overwrite;
        bool overwriteCacheOnError = settings.LocalCache.OverwriteOnError;
        string path = Path.Join(settings.LocalCache.Directory.LocalPath, relativeUri);
        var fileExists = File.Exists(path);

        if (!fileExists || overwriteCache || !cacheEnabled)
        {
            logger.LogDebug("Getting the data from the api {fileExists} {overwriteCache} {cacheEnabled}", fileExists, overwriteCache, cacheEnabled);
            return await FromApi<T>(relativeUrl, path, cancellationToken);
        }

        try
        {
            logger.LogInformation(
                "Retrieving data from local source instead of API. Url: {relativeUrl} Uri: {relativeUri}",
                relativeUrl, relativeUri);
            return await reader.Read<T>(path, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Exception caught while trying to read a local data file {overwriteCacheOnError}", overwriteCacheOnError);
            if (overwriteCacheOnError)
            {
                return await FromApi<T>(relativeUrl, path, cancellationToken);
            }
            throw;
        }
    }
}