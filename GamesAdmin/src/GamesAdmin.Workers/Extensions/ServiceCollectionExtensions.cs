using GamesAdmin.Workers.Bots;
using GamesAdmin.Workers.Bots.Tasks;
using GamesAdmin.Workers.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace GamesAdmin.Workers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            var mongoClient = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            services.AddSingleton<IMongoClient>(mongoClient);            
            services.AddSingleton(mongoClient.GetDatabase(configuration["DatabaseSettings:DatabaseName"]));
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IKenoService, KenoService>();
            services.AddScoped<IBotService, BotService>();
        }

        public static void AddScheduleTasks(this IServiceCollection services)
        {
            services.AddTransient<ReviseBotBalanceTask>();
            services.AddTransient<DisableBotTask>();
        }
    }
}