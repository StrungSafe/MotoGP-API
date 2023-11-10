using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotoGP.Api;
using MotoGP.Configuration;
using MotoGP.Utilities;

namespace MotoGP.Analyzer;

public class DataAnalyzer : IDataAnalyzer
{
    private readonly MachineLearning settings;

    private readonly ILogger<DataAnalyzer> logger;

    private readonly IDataReader reader;

    public DataAnalyzer(ILogger<DataAnalyzer> logger, IOptions<MachineLearning> settings, IDataReader reader)
    {
        this.logger = logger;
        this.settings = settings.Value;
        this.reader = reader;
    }

    public async Task AnalyzeData()
    {
        logger.LogInformation("Analyzing the seasons.json data...");
        var path = Path.Join(settings.Objects.LocalPath, "seasons.json");
        Season[] seasons = await reader.Read<Season[]>(path);

        var categoryTypes = new HashSet<string>();
        var sessionTypes = new HashSet<string>();
        var eventTypes = new HashSet<string>();
        var weatherTypes = new HashSet<string>();
        var recordTypes = new HashSet<string>();
        var eventNames = new HashSet<string>();
        var testEventNames = new HashSet<string>();
        var riderNames = new HashSet<string>();
        var circuitNames = new HashSet<string>();

        foreach (Season season in seasons)
        {
            foreach (Event _event in season.Events)
            {
                if(_event.Test)
                {
                    testEventNames.Add(_event.Name);
                } else
                {
                    eventNames.Add(_event.Name);
                }

                foreach (Category category in _event.Categories)
                {
                    categoryTypes.Add(category.Name);

                    foreach (Session session in category.Sessions)
                    {
                        circuitNames.Add(session.Circuit);
                        sessionTypes.Add(session.Type);

                        eventTypes.Add(session.Condition.Track);
                        weatherTypes.Add(session.Condition.Weather);

                        foreach (Classification classification in session.SessionClassification.Classifications)
                        {
                            riderNames.Add(classification.Rider.FullName);
                        }

                        foreach (Record record in session.SessionClassification.Records)
                        {
                            recordTypes.Add(record.Type);
                        }
                    }
                }
            }
        }

        string Join(IEnumerable<string> values, string separator = ", ")
        {
            return string.Join(separator, values);
        }

        logger.LogInformation("Category Types: {categoryTypes}", Join(categoryTypes));
        logger.LogInformation("Session Types: {sessionTypes}", Join(sessionTypes));
        logger.LogInformation("Event Types: {eventTypes}", Join(eventTypes));
        logger.LogInformation("Weather Types: {weatherTypes}", Join(weatherTypes));
        logger.LogInformation("Record Types: {recordTypes}", Join(recordTypes));
        logger.LogInformation("Event Names ({eventCount}): {eventNames}", eventNames.Count,
            Join(eventNames, $", {Environment.NewLine}"));
        logger.LogInformation("Test Event Names ({eventCount}): {eventNames}", testEventNames.Count,
            Join(testEventNames, $", {Environment.NewLine}"));
        logger.LogInformation("Riders ({riderCount}): {riderNames}", riderNames.Count,
            Join(riderNames, $", {Environment.NewLine}"));
        logger.LogInformation("Circuit Names ({circuitCount}): {circuitNames}", circuitNames.Count, Join(circuitNames, $", {Environment.NewLine}"));

        await Task.Delay(100); //TODO logger not flushing...there is a way to flush but need to find out again
    }
}