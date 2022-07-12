using GamesAdmin.Api._Shared.Configurations;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
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


            var decimalSerializer = new DecimalSerializer(BsonType.Decimal128, new RepresentationConverter(allowOverflow: false, allowTruncation: true));
            BsonSerializer.RegisterSerializer(decimalSerializer);
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            var mongoClient = new MongoClient(appSettings.DatabaseSettings.ConnectionString);
            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton(mongoClient.GetDatabase(appSettings.DatabaseSettings.DatabaseName));
        }
    }
}