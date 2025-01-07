using GamesAdmin.Core;
using System.Collections.Generic;

namespace GamesAdmin.Site._Shared.Configurations
{
    public interface ISystemSettings
    {
        string System { get; }
        string BaseCurrency { get; }

        int BaseCurrencyMultiplier { get; }
    }

    public class SystemSettings : ISystemSettings
    {
        public SystemSettings(string systemType)
        {
            System = systemType;
            BaseCurrency = BaseCurrencyMapper[System];
            BaseCurrencyMultiplier = BaseCurrencyMultiplierMapper[System];
        }

        private readonly Dictionary<string, string> BaseCurrencyMapper = new Dictionary<string, string>
        {
            [SystemType.SC1] = "USD",
            [SystemType.VG2] = "INH",
            [SystemType.GN] = "USD",
            [SystemType.HD] = "USD",
        };

        private readonly Dictionary<string, int> BaseCurrencyMultiplierMapper = new Dictionary<string, int>
        {
            [SystemType.SC1] = 1,
            [SystemType.VG2] = 1000,
            [SystemType.GN] = 1000,
            [SystemType.HD] = 1,
        };

        public string System { get; }
        public string BaseCurrency { get; }
        public int BaseCurrencyMultiplier { get; }
    }
}