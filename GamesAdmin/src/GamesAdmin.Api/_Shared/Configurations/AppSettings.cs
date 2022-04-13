using GamesAdmin.Core.IdentityServer;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;

namespace GamesAdmin.Api._Shared.Configurations
{
    public interface IAppSettings
    {
        IdentityServerAuth IdentityServerAuth { get; }

        GameServerSettings GameServers { get; }

        SabaApiSettings SabaApiSettings { get; }

        DatabaseSettings DatabaseSettings { get; }

        ArchiveDatabaseSettings ArchiveDatabaseSettings { get; }

        RemovedDatabaseSettings RemovedDatabaseSettings { get; }

        ChatBotApiSettings ChatBotApiSettings { get; }

        ApiLogDatabaseSettings ApiLogDatabaseSettings { get; }

        AdminMonitoringSettings AdminMonitoringSettings { get; }
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            GameServers = configuration.GetSection(nameof(GameServers)).Get<GameServerSettings>();
            IdentityServerAuth = configuration.GetSection(nameof(IdentityServerAuth)).Get<IdentityServerAuth>();
            SabaApiSettings = configuration.GetSection(nameof(SabaApiSettings)).Get<SabaApiSettings>();
            DatabaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            ArchiveDatabaseSettings = configuration.GetSection(nameof(ArchiveDatabaseSettings)).Get<ArchiveDatabaseSettings>();
            RemovedDatabaseSettings = configuration.GetSection(nameof(RemovedDatabaseSettings)).Get<RemovedDatabaseSettings>();
            ChatBotApiSettings = configuration.GetSection(nameof(ChatBotApiSettings)).Get<ChatBotApiSettings>();
            ApiLogDatabaseSettings = configuration.GetSection(nameof(ApiLogDatabaseSettings)).Get<ApiLogDatabaseSettings>();
            AdminMonitoringSettings = configuration.GetSection(nameof(AdminMonitoringSettings)).Get<AdminMonitoringSettings>();
        }

        public IdentityServerAuth IdentityServerAuth { get; }

        public GameServerSettings GameServers { get; }

        public SabaApiSettings SabaApiSettings { get; }

        public DatabaseSettings DatabaseSettings { get; }

        public ArchiveDatabaseSettings ArchiveDatabaseSettings { get; }

        public RemovedDatabaseSettings RemovedDatabaseSettings { get; }

        public ChatBotApiSettings ChatBotApiSettings { get; }

        public ApiLogDatabaseSettings ApiLogDatabaseSettings { get; }

        public AdminMonitoringSettings AdminMonitoringSettings { get; }

        public T GetValue<T>(string key)
        {
            try
            {
                var value = configuration[key];

                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));

                return (T)typeConverter.ConvertFromString(value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Key: {key}", ex);
            }
        }
    }
}