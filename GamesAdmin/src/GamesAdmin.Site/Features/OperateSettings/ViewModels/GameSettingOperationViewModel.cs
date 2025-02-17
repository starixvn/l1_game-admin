using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using Newtonsoft.Json;

namespace GamesAdmin.Site.Features.OperateSettings.ViewModels
{
    public class GameSettingOperationViewModel
    {
        public IEnumerable<GameConfig> GameConfigs { get; set; }

        public string BuildSetting(GameConfig gameConfig)
        {
            return JsonConvert.SerializeObject(gameConfig, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            });
        }
    }
}