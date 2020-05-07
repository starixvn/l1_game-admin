using System;
using System.Collections.Generic;
using System.Text;
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
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention(), new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            var mongoClient = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            services.AddSingleton<IMongoClient>(mongoClient);
        }
    }
}