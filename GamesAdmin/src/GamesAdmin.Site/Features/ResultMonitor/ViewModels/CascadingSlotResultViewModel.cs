using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.ResultMonitor.ViewModels
{
    public class CascadingSlotResultViewModel
    {
        private const int numberOfTumbling = 4;

        public CascadingSlotResultViewModel()
        {
        }

        public CascadingSlotResultViewModel(string name)
        {
            GameName = name;
        }

        public string GameName { get; set; }

        public string DisplayGameName => Enumeration.FromValue<Game>(GameName).DisplayName;

        public string Nickname { get; set; }

        [DisplayName("Base Spin")]
        public string BaseSpin { get; set; }

        public List<string> Tumblings { get; set; } = Enumerable.Repeat(string.Empty, numberOfTumbling).ToList();
    }
}