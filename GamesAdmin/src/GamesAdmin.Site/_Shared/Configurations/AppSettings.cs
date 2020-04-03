using System;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace GamesAdmin.Site._Shared.Configurations
{
    public interface IAppSettings 
    {
        string ApiHost { get; }

        string Hash { get; }
    }
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        public AppSettings(IConfiguration configuration) 
        {
            this.configuration = configuration;

            ApiHost = GetValue<string>(nameof(ApiHost));
            Hash = GetValue<string>(nameof(Hash));
        }

        public string ApiHost { get; }

        public string Hash { get; }

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
