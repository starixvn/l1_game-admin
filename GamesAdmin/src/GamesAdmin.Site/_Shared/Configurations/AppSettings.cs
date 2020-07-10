using System;
using System.ComponentModel;
using GamesAdmin.Site._Shared.IdentityServer;
using Microsoft.Extensions.Configuration;

namespace GamesAdmin.Site._Shared.Configurations
{
    public interface IAppSettings
    {
        string ApiHost { get; }

        IdentityServerAuth IdentityServerAuth { get; }

        string GameServerUrl { get; }

        string GameServerDomainUrl { get; }

        TimeZoneInfo DefaultTimeZone { get; }

        string DefaultUmMessage { get; }

        string Env { get; }

        string ResultPageUrl { get; }

        int MaxDecreased { get; }
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            ApiHost = GetValue<string>(nameof(ApiHost));
            GameServerUrl = GetValue<string>(nameof(GameServerUrl));
            GameServerDomainUrl = GetValue<string>(nameof(GameServerDomainUrl));
            IdentityServerAuth = configuration.GetSection(nameof(IdentityServerAuth)).Get<IdentityServerAuth>();
            DefaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetValue<string>(nameof(DefaultTimeZone)));
            DefaultUmMessage = GetValue<string>(nameof(DefaultUmMessage));
            Env = GetValue<string>(nameof(Env));
            ResultPageUrl = GetValue<string>(nameof(ResultPageUrl));
            MaxDecreased = GetValue<int>(nameof(MaxDecreased));
        }

        public string ApiHost { get; }

        public IdentityServerAuth IdentityServerAuth { get; }

        public TimeZoneInfo DefaultTimeZone { get; }

        public string GameServerUrl { get; }

        public string DefaultUmMessage { get; }

        public string GameServerDomainUrl { get; }

        public string Env { get; }

        public string ResultPageUrl { get; }

        public int MaxDecreased { get; }

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