using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.ResultMonitor.ViewModels
{
    public class SlotResultViewModel
    {
        public SlotResultViewModel()
        {
        }

        public SlotResultViewModel(string name)
        {
            GameName = name;
        }

        public string GameName { get; set; }

        public string DisplayGameName => Enumeration.FromValue<Game>(GameName).DisplayName;

        public string Nickname { get; set; }

        public string Results { get; set; }
    }
}