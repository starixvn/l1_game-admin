using System.IO;
using Coravel;
using GamesAdmin.Site._Shared.Middlewares;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sentry;

namespace GamesAdmin.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            host.Services.UseScheduler(scheduler =>
            {
                scheduler.AddScheduleTasks();
            })
            .OnError((exception) => SentrySdk.CaptureException(exception));

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .Build();

            return WebHost.CreateDefaultBuilder(args)
               .UseKestrel()
               .UseStartup<Startup>()
               .UseSentry(opt => opt.Environment = configuration["Env"])
               .UseIISIntegration()
               .UseUrls($"http://localhost:{configuration["HostingPort"]}");
        }
    }
}
