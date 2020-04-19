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
        public StatusItemViewModel(string name, bool enabled) 
        {
            Name = name;
            Enabled = enabled;
        }

        public string Name { get; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? string.Empty : Enumeration.FromValue<GameType>(Name).DisplayName;

        public bool Enabled { get; }

        public string DisabledMessage { get; }
    }
}
