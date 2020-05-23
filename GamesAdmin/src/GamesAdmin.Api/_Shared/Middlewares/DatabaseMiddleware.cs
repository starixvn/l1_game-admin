using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace GamesAdmin.Api._Shared.Middlewares
{
    public static class DatabaseMiddleware
    {
        public static void AddDatabase(this IServiceCollection services, IConfigurationSection dbSettings) 
        {
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true) ,
                new IgnoreIfDefaultConvention(true)
            };

            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            var mongoClient = new MongoClient(dbSettings["ConnectionString"]);
            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton(mongoClient.GetDatabase(dbSettings["DatabaseName"]));
        }
    }
}
