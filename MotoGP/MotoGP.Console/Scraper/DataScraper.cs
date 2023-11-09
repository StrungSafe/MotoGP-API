using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotoGP.Api;
using MotoGP.Configuration;
using MotoGP.Data;
using MotoGP.Utilities;

namespace MotoGP.Scraper;

public class DataScraper : IDataScraper
{
    private readonly IDataLoader loader;

    private readonly ILogger<DataScraper> logger;

    private readonly MachineLearning settings;

    private readonly IDataWriter writer;

    public DataScraper(ILogger<DataScraper> logger, IDataLoader loader, IDataWriter writer,
        IOptions<MachineLearning> settings)
    {
        this.logger = logger;
        this.loader = loader;
        this.writer = writer;
        this.settings = settings.Value;
    }

    public async Task Scrape(CancellationToken token)
    {
        IEnumerable<Season> seasons = await loader.Load(token);
        await Task.WhenAll(WriteSeasons(seasons, token),
            WriteRiders(seasons, token),
            WriteCircuits(seasons, token));
    }

    private async Task WriteCircuits(IEnumerable<Season> seasons, CancellationToken token)
    {
        Dictionary<int, string> circuits = seasons.SelectMany(s => s.Events).SelectMany(e => e.Categories)
                                                       .SelectMany(c => c.Sessions)
                                                       .DistinctBy(c => c.Circuit)
                                                       .Select(c => c.Circuit)
                                                       .Select((value, index) => new { Index = index, Value = value })
                                                       .ToDictionary(item => item.Index, item => item.Value);
        string path = Path.Join(settings.Objects.LocalPath, "circuits.json");
        await writer.Write(path, circuits, token);
    }

    private async Task WriteRiders(IEnumerable<Season> seasons, CancellationToken token)
    {
        Dictionary<int, string> riders = seasons.SelectMany(s => s.Events).SelectMany(e => e.Categories)
                                               .SelectMany(c => c.Sessions).SelectMany(s =>
                                                   s.SessionClassification.Classifications)
                                               .DistinctBy(c => c.Rider.FullName)
                                               .Select(c => c.Rider.FullName)
                                               .Select((value, index) => new { Index = index, Value = value })
                                               .ToDictionary(item => item.Index, item => item.Value);
        string path = Path.Join(settings.Objects.LocalPath, "riders.json");
        await writer.Write(path, riders, token);
    }

    private Task WriteSeasons(IEnumerable<Season> seasons, CancellationToken token)
    {
        string path = Path.Join(settings.Objects.LocalPath, "seasons.json");
        return writer.Write(path, seasons, token);
    }
}