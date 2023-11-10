namespace MotoGP.Scraper;

public interface IDataScraper
{
    Task Scrape(CancellationToken token = default);
}