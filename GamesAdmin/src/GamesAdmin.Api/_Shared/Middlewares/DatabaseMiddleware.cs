using GamesAdmin.Api._Shared.Configurations;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace GamesAdmin.Api._Shared.Middlewares
{
    public static class DatabaseMiddleware
    {
        public static void AddDatabase(this IServiceCollection services, IAppSettings appSettings)
        {
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            var mongoClient = new MongoClient(appSettings.DatabaseSettings.ConnectionString);
            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton(mongoClient.GetDatabase(appSettings.DatabaseSettings.DatabaseName));
        }
    }
}