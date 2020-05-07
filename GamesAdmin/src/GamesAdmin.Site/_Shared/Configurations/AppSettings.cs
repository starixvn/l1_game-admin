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

        TimeZoneInfo DefaultTimeZone { get; }

        BetSettings BetSettings { get; }
    }
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            ApiHost = GetValue<string>(nameof(ApiHost));
            GameServerUrl = GetValue<string>(nameof(GameServerUrl));
            IdentityServerAuth = configuration.GetSection(nameof(IdentityServerAuth)).Get<IdentityServerAuth>();
            DefaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById(GetValue<string>(nameof(DefaultTimeZone)));

            BetSettings = configuration.GetSection(nameof(BetSettings)).Get<BetSettings>();
        }

        public string ApiHost { get; }

        public IdentityServerAuth IdentityServerAuth { get; }

        public TimeZoneInfo DefaultTimeZone { get; }

        public string GameServerUrl { get; }

        public BetSettings BetSettings { get; }

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
