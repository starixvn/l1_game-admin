using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Features.OperateSettings.Models;
using GamesAdmin.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Features.OperateSettings
{
    [Route("api/operate_settings")]
    public class OperateSettingsController : BaseAuthorizeController
    {
        private readonly IOperateSettingsService _operateSettingsService;

        public OperateSettingsController(IOperateSettingsService operateSettingsService)
        {
            _operateSettingsService = operateSettingsService;
        }

        [HttpPost("game/settings")]
        public async Task<IEnumerable<GameConfig>> GetOperateGameSettings([FromBody]IList<int> gameIds)
        {
            var operateGameSettings = await _operateSettingsService.GetOperateGameSettingsAsync(gameIds);

            return operateGameSettings.GameConfigs;
        }
    }
} 