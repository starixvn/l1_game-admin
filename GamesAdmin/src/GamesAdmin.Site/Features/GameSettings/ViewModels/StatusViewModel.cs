using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site._Shared.Enumerations;

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
        public StatusItemViewModel() { }

        public StatusItemViewModel(string name, bool enabled, string disabledMessage) 
        {
            Name = name;
            Enabled = enabled;
            DisabledMessage = disabledMessage;
        }

        public string Name { get; set; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? string.Empty : Enumeration.FromValue<GameType>(Name).DisplayName;

        public bool Enabled { get; set; }

        public string DisabledMessage { get; set; }
    }

    public class GameDisableMessage 
    {
        public string Name { get; set; }

        public string DisabledMessage { get; set; }
    }
}
