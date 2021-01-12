using System;
using System.ComponentModel;
using GamesAdmin.Core.IdentityServer;
using Microsoft.Extensions.Configuration;

namespace GamesAdmin.Api._Shared.Configurations
{
    public interface IAppSettings
    {
        IdentityServerAuth IdentityServerAuth { get; }

        GameServerSettings GameServers { get; }

        SabaApiSettings SabaApiSettings { get; }
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
        }

        public IdentityServerAuth IdentityServerAuth { get; }

        public GameServerSettings GameServers { get; }

        public SabaApiSettings SabaApiSettings { get; }

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