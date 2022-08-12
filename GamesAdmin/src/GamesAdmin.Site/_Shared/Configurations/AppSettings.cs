using System;
using System.ComponentModel;
using GamesAdmin.Core.IdentityServer;
using Microsoft.Extensions.Configuration;

namespace GamesAdmin.Site._Shared.Configurations
{
    public interface IAppSettings
    {
        string SystemType { get; }

        string ApiHost { get; }

        IdentityServerAuth IdentityServerAuth { get; }

        TimeZoneInfo DefaultTimeZone { get; }

        string DefaultUmMessage { get; }

        string Env { get; }

        int MaxDecreased { get; }

        ResultPageConfiguration ResultPageConfiguration { get; }

        GameServerSettings GameServers { get; }

        string GameApiUrl { get; }

        string SGApiUrl { get; }
    }

    public class ResultPageConfiguration
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientKey { get; set; }
        public string Language { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            ApiHost = GetValue<string>(nameof(ApiHost));
            GameServers = configuration.GetSection(nameof(GameServers)).Get<GameServerSettings>();
            IdentityServerAuth = configuration.GetSection(nameof(IdentityServerAuth)).Get<IdentityServerAuth>();
            DefaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetValue<string>(nameof(DefaultTimeZone)));
            DefaultUmMessage = GetValue<string>(nameof(DefaultUmMessage));
            Env = GetValue<string>(nameof(Env));
            SystemType = GetValue<string>(nameof(SystemType));
            ResultPageConfiguration = configuration.GetSection(nameof(ResultPageConfiguration)).Get<ResultPageConfiguration>();
            MaxDecreased = GetValue<int>(nameof(MaxDecreased));
            GameApiUrl = GetValue<string>(nameof(GameApiUrl));
            SGApiUrl = GetValue<string>(nameof(SGApiUrl));
        }

        public string ApiHost { get; }

        public IdentityServerAuth IdentityServerAuth { get; }

        public TimeZoneInfo DefaultTimeZone { get; }

        public string DefaultUmMessage { get; }

        public string Env { get; }

        public ResultPageConfiguration ResultPageConfiguration { get; }

        public int MaxDecreased { get; }

        public GameServerSettings GameServers { get; }

        public string GameApiUrl { get; }

        public string SGApiUrl { get; }
        
        public string SystemType { get; }

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