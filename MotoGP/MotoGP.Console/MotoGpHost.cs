using Microsoft.Extensions.Logging;
using MotoGP.Analyzer;
using MotoGP.Scraper;
using System;

namespace MotoGP.Console
{
    public class MotoGpHost
    {
        private readonly IDataAnalyzer analyzer;

        private readonly ILogger<MotoGpHost> logger;


        private readonly IDataScraper scraper;

        public MotoGpHost(ILogger<MotoGpHost> logger, IDataScraper scraper, IDataAnalyzer analyzer)
        {
            this.logger = logger;
            this.scraper = scraper;
            this.analyzer = analyzer;
        }

        public async Task Run()
        {
            var input = string.Empty;
            int service;
            do
            {
                try
                {
                    System.Console.WriteLine("Enter a # and enter to start a service");
                    System.Console.WriteLine("0. Exit");
                    System.Console.WriteLine("1. Scraper");
                    System.Console.WriteLine("2. Analyzer");
                    input = System.Console.ReadLine();
                    if (int.TryParse(input, out service))
                    {
                        switch (service)
                        {
                            case 1:
                                await scraper.Scrape();
                                break;
                            case 2:
                                await analyzer.AnalyzeData();
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    service = -1;
                    logger.LogError(ex, "There was an unhandled exception while running the service {serviceEncoded}",
                        service);
                    System.Console.WriteLine("***************");
                    System.Console.WriteLine();
                }
            } while (!string.Equals("exit", input) && service != 0);
        }
    }
}