using GamesAdmin.Workers.Configuration;
using GamesAdmin.Workers.Extensions;
using GamesAdmin.Workers.Repository;
using GamesAdmin.Workers.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GamesAdmin.Workers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<VietlottKenoConfiguration>(hostContext.Configuration.GetSection(nameof(VietlottKenoConfiguration)));
                    services.AddMongoDatabase(hostContext.Configuration);
                    services.AddTransient<IKenoService, KenoService>();
                    services.AddTransient<IKenoRepository, KenoRepository>();
                    services.AddHostedService<VietlottKenoWorker>();
                });
    }
}