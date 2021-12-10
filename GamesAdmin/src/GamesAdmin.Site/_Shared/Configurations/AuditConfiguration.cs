using System;
using Audit.Core;
using Microsoft.Extensions.Configuration;

namespace GamesAdmin.Site._Shared.Configurations
{
    public static class AuditConfiguration
    {
        private const string DatabaseConnectionStringKey = "AuditDatabaseSettings:ConnectionString";
        private const string DatabaseNameKey = "AuditDatabaseSettings:DatabaseName";
        private const string CollectionName = "audit_events";

        public static void Init(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new InvalidOperationException("Configuration cannot be null");
            }

            var auditConnectionString = configuration[DatabaseConnectionStringKey];

            if(string.IsNullOrWhiteSpace(auditConnectionString))
            {
                throw new InvalidOperationException("Audit Connectionstring cannot be null");
            }    

            Configuration.Setup()
                .UseMongoDB(config => config
                .ConnectionString(auditConnectionString)
                .Database(configuration[DatabaseNameKey])
                .Collection(CollectionName));
        }
    }
}