using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MotoGP.Extensions;

namespace MotoGP
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<MotoGpHost>();

            IHost host = builder.AddAll().Build();

            var motoGp = host.Services.GetRequiredService<MotoGpHost>();

            await motoGp.Run();
        }
    }
}