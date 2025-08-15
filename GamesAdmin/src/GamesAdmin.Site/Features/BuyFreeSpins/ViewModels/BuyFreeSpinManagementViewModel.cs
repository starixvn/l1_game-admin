using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Site.Features.BuyFreeSpins.ViewModels
{
    public class BuyFreeSpinManagementViewModel
    {
        public IEnumerable<BuyFreeSpinSettingViewModel> Models { get; set; }
    }

    public class BuyFreeSpinSettingViewModel : BuyFreeSpinSetting
    {
        public string GameName { get; set; }
        public int GameId { get; set; }
    }
}