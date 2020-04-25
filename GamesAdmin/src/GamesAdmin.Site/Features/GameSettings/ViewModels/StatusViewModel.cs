using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site._Shared.Enumerations;
using GameType = GamesAdmin.Site._Shared.Enumerations.GameType;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class StatusViewModel
    {
        public StatusViewModel(IList<StatusItemViewModel> gameList) 
        {
            GameList = gameList;
        }

        public IList<StatusItemViewModel> GameList { get; }
    }

    public class StatusItemViewModel 
    {
        public StatusItemViewModel(string name, bool enabled, string disabledMessage) 
        {
            Name = name;
            Enabled = enabled;
            DisabledMessage = disabledMessage;
        }

        public string Name { get; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? string.Empty : Enumeration.FromValue<GameType>(Name).DisplayName;

        public bool Enabled { get; }

        public string StatusDisplay => Enabled ? "Yes" : "No";

        public string DisabledMessage { get; }
    }

    public class GameDisableMessage 
    {
        public string Name { get; set; }

        public string DisabledMessage { get; set; }
    }
}
