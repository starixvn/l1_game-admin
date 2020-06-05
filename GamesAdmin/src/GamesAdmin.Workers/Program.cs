using Coravel;
using GamesAdmin.Database;
using GamesAdmin.VietlottKeno;
using GamesAdmin.Workers._Shared;
using GamesAdmin.Workers._Shared.Configurations;
using GamesAdmin.Workers.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GamesAdmin.Workers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            host.Services.UseScheduler(scheduler =>
            {
                scheduler.AddScheduleTasks();
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<VietlottKenoConfiguration>(hostContext.Configuration.GetSection(nameof(VietlottKenoConfiguration)));

                    services.AddMongoDatabase(hostContext.Configuration);
                    services.AddServices();

                    services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));

                    services.AddHostedService<VietlottKenoWorker>();

                    services.AddScheduler();
                    services.AddScheduleTasks();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration);
                    logging.AddSentry();
                });
    }
}