using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class MarginSettingViewModel
    {
        public MarginSettingViewModel()
        { }

        public MarginSettingViewModel(Dictionary<string, double> marginSetting)
        {
            MarginSetting = marginSetting;
        }

        public Dictionary<string, double> MarginSetting;
    }
}