using Coravel;
using GamesAdmin.Workers.Bots.Tasks;
using GamesAdmin.Workers.Configuration;
using GamesAdmin.Workers.Extensions;
using GamesAdmin.Workers.Repository;
using GamesAdmin.Workers.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GamesAdmin.Workers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            host.Services.UseScheduler(scheduler => {

                scheduler
                    .Schedule<ReviseBotBalanceTask>()
                    .EveryThirtyMinutes();
            });

            host.Run();
        }

        //TODO inject entity, inject services
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<VietlottKenoConfiguration>(hostContext.Configuration.GetSection(nameof(VietlottKenoConfiguration)));
                    services.AddMongoDatabase(hostContext.Configuration);
                    services.AddTransient<IKenoService, KenoService>();
                    services.AddTransient<IKenoRepository, KenoRepository>();
                    services.AddHostedService<VietlottKenoWorker>();

                    services.AddScheduler();
                    services.AddTransient<ReviseBotBalanceTask>();
                })
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddConfiguration(hostingContext.Configuration);
                    logging.AddSentry();
                });
    }
}