using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GlobalConfigs.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GlobalConfigs
{
    [Route("api/global_configs")]
    public class GlobalConfigController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public GlobalConfigController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("get_exchange_codes")]
        public Task<GlobalConfig> GetExchangeCodes()
            => mediator.Send(new GetGlobalConfigRequest(GlobalConfigType.ExchangeCodes));
    }
}