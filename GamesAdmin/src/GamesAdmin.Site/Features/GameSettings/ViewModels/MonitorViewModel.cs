using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class MonitorViewModel
    {
        public MonitorViewModel(IList<MonitorItemViewModel> gameList)
        {
            GameList = gameList;
        }

        public IList<MonitorItemViewModel> GameList { get; }
    }

    public class MonitorItemViewModel
    {

        private static string[] disableHistory = new string[]
        {
        };

        private static string[] disableMonitoring = new string[]
        {
            Game.LuckyWheelValue,
            Game.KenoProVietValue,
            Game.BlackJackValue
        };

        public MonitorItemViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) || !Enumeration.Contains<Game>(Name)
            ? Name
            : Enumeration.FromValue<Game>(Name).DisplayName;

        public bool EnabledMonitoring => !disableMonitoring.Contains(Name);

        public bool EnabledHistory => !disableHistory.Contains(Name);

        public string GameHistoryUrl => DisplayName.Replace(" ", string.Empty).Trim().ToLowerInvariant();
    }
}
