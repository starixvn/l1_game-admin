using GamesAdmin.Core;
using System.Collections.Generic;

namespace GamesAdmin.Site._Shared.Configurations
{

    public interface ISystemSettings
    {
        string System { get; }
        string BaseCurrency { get; }
    }

    public class SystemSettings : ISystemSettings
    {
        public SystemSettings(string systemType)
        {
            System = systemType;
            BaseCurrency = BaseCurrencyMapper[System];
        }

        private readonly Dictionary<string, string> BaseCurrencyMapper = new Dictionary<string, string>
        {
            [SystemType.SC1] = "USD",
            [SystemType.VG2] = "INH",
        };

        public string System { get; }
        public string BaseCurrency { get; }
    }
}