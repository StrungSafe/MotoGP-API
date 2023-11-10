using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MotoGP.Analyzer;
using MotoGP.Configuration;
using MotoGP.Http;
using MotoGP.Data;
using MotoGP.Repositories;
using MotoGP.Scraper;
using MotoGP.Utilities;

namespace MotoGP.Extensions
{
    public static class HostApplicationBuilderExtensions
    {
        public static HostApplicationBuilder AddAll(this HostApplicationBuilder builder)
        {
            return builder.AddMotoGp().AddScraper().AddAnalyzer();
        }

        public static HostApplicationBuilder AddAnalyzer(this HostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IDataAnalyzer, DataAnalyzer>();
            return builder;
        }

        public static HostApplicationBuilder AddMotoGp(this HostApplicationBuilder builder)
        {
            builder.AddUtilities()
                   .Services
                   .AddOptions()
                   .Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)))
                   .Configure<Repository>(builder.Configuration.GetSection(nameof(Repository)))
                   .AddSingleton<IDataRepository, DataRepository>()
                   .AddSingleton<IDataLoader, DataLoader>()
                   .AddSingleton<ThrottlingDelegatingHandler>()
                   .AddHttpClient<MotoGpClient>((provider, client) =>
                   {
                       var settings = provider.GetRequiredService<IOptions<Repository>>();
                       client.BaseAddress = new Uri(settings.Value.Client.BaseAddress, UriKind.Absolute);
                   })
                   .AddHttpMessageHandler<ThrottlingDelegatingHandler>();

            return builder;
        }

        public static HostApplicationBuilder AddScraper(this HostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IDataScraper, DataScraper>();
            return builder;
        }

        public static HostApplicationBuilder AddUtilities(this HostApplicationBuilder builder)
        {
            builder.Services
                   .AddSingleton<IDataWriter, JsonDataService>()
                   .AddSingleton<IDataReader, JsonDataService>();

            return builder;
        }
    }
}