using System;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Api.BuyFreeSpins.ViewModels
{
    public class BuyFreeSpinSettingViewModel
    {
        public BuyFreeSpinSettingViewModel()
        {
        }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public bool Enabled { get; set; }

        public int Multiplier { get; set; }

        public BuyFreeSpinSiteExclusion[] SiteExclusions { get; set; } = Array.Empty<BuyFreeSpinSiteExclusion>();

    }
}