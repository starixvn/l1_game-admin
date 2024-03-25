using System;
using System.ComponentModel;
using GamesAdmin.Core.IdentityServer;
using Microsoft.Extensions.Configuration;

namespace GamesAdmin.Api._Shared.Configurations
{
    public interface IAppSettings
    {
        string SystemType { get; }

        string Env { get; }

        public string ServiceApiTemplate { get; }

        IdentityServerAuth IdentityServerAuth { get; }

        GameServerSettings GameServers { get; }

        SabaApiSettings SabaApiSettings { get; }

        DatabaseSettings DatabaseSettings { get; }

        ArchiveDatabaseSettings ArchiveDatabaseSettings { get; }

        RemovedDatabaseSettings RemovedDatabaseSettings { get; }

        ChatBotApiSettings ChatBotApiSettings { get; }

        ApiLogDatabaseSettings ApiLogDatabaseSettings { get; }

        AdminMonitoringSettings AdminMonitoringSettings { get; }

        RtpCheckerServiceSettings RtpCheckerServiceSettings { get; }

        RoundWatcherServiceSettings RoundWatcherServiceSettings { get; }

        StatusCheckerServiceSettings StatusCheckerServiceSettings { get; }

        CloudSettings CloudSettings { get; }

        string ProxyUrl { get; }

        string ServiceNameSeparator { get; }
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            SystemType = GetValue<string>(nameof(SystemType));
            Env = GetValue<string>(nameof(Env));
            GameServers = configuration.GetSection(nameof(GameServers)).Get<GameServerSettings>();
            IdentityServerAuth = configuration.GetSection(nameof(IdentityServerAuth)).Get<IdentityServerAuth>();
            SabaApiSettings = configuration.GetSection(nameof(SabaApiSettings)).Get<SabaApiSettings>();
            DatabaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            ArchiveDatabaseSettings = configuration.GetSection(nameof(ArchiveDatabaseSettings)).Get<ArchiveDatabaseSettings>();
            RemovedDatabaseSettings = configuration.GetSection(nameof(RemovedDatabaseSettings)).Get<RemovedDatabaseSettings>();
            ChatBotApiSettings = configuration.GetSection(nameof(ChatBotApiSettings)).Get<ChatBotApiSettings>();
            ApiLogDatabaseSettings = configuration.GetSection(nameof(ApiLogDatabaseSettings)).Get<ApiLogDatabaseSettings>();
            AdminMonitoringSettings = configuration.GetSection(nameof(AdminMonitoringSettings)).Get<AdminMonitoringSettings>();
            ProxyUrl = configuration[nameof(ProxyUrl)] ?? GetValue<string>(nameof(ProxyUrl));
            RoundWatcherServiceSettings = configuration.GetSection(nameof(RoundWatcherServiceSettings)).Get<RoundWatcherServiceSettings>();
            ServiceApiTemplate = BuildSeviceApiTemplate(configuration["GameServiceTemplate"]);
            RtpCheckerServiceSettings = configuration.GetSection(nameof(RtpCheckerServiceSettings)).Get<RtpCheckerServiceSettings>();
            StatusCheckerServiceSettings = configuration.GetSection(nameof(StatusCheckerServiceSettings)).Get<StatusCheckerServiceSettings>();
            CloudSettings = configuration.GetSection(nameof(CloudSettings)).Get<CloudSettings>();
            ServiceNameSeparator = configuration["ServiceNameSeparator"];
        }

        public string SystemType { get; }

        public string Env { get; }

        public string ServiceApiTemplate { get; }

        public string ProxyUrl { get; }

        public IdentityServerAuth IdentityServerAuth { get; }

        public GameServerSettings GameServers { get; }

        public SabaApiSettings SabaApiSettings { get; }

        public DatabaseSettings DatabaseSettings { get; }

        public ArchiveDatabaseSettings ArchiveDatabaseSettings { get; }

        public RemovedDatabaseSettings RemovedDatabaseSettings { get; }

        public ChatBotApiSettings ChatBotApiSettings { get; }

        public ApiLogDatabaseSettings ApiLogDatabaseSettings { get; }

        public AdminMonitoringSettings AdminMonitoringSettings { get; }

        public RtpCheckerServiceSettings RtpCheckerServiceSettings { get; }

        public RoundWatcherServiceSettings RoundWatcherServiceSettings { get; }

        public StatusCheckerServiceSettings StatusCheckerServiceSettings { get; }

        public CloudSettings CloudSettings { get; }

        public string ServiceNameSeparator { get; }

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

        private string BuildSeviceApiTemplate(string template)
        {
            if (ProxyUrl != String.Empty)
            {
                return $"{ProxyUrl}{{0}}";
            }
            else
            {
                var servicePrefix = SystemType == Core.SystemType.SC1 ? "l1" : "l2";

                return template ?? $"http://{servicePrefix}_{{0}}_service_{Env}:5000";
            }
        }
    }
}