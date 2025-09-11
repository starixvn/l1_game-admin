using System.Collections.Generic;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Api.Features.OperateSettings.Models
{
    public class OperateGameSettings
    {
        public IList<GameConfig> GameConfigs { get; set; }
    }
}
