using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.ResultMonitor.ViewModels
{
    public class HoldemPokerResultViewModel
    {
        public HoldemPokerResultViewModel(string name)
        {
            GameName = name;
        }

        public string GameName { get; }

        public string DisplayGameName => Enumeration.FromValue<Game>(GameName).DisplayName;
    }
}