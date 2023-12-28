using System;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Api.VoidGameRemote;
using GamesAdmin.Core.IdentityServer;
using GamesAdmin.Core.OWResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.VoidGame
{
    [Route("api/void")]
    public class VoidGameController : BaseAuthorizeController
    {
        private readonly IMediator mediator;
        private readonly IAppSettings appSettings;
        private readonly IServiceProvider serviceProvider;

        public VoidGameController(IMediator mediator, IAppSettings appSettings, IServiceProvider serviceProvider)
        {
            this.mediator = mediator;
            this.appSettings = appSettings;
            this.serviceProvider = serviceProvider;
        }

        [HttpPost]
        public async Task<VoidGameResult> VoidTicket([FromBody] VoidTicketRequest request)
        {
            if (appSettings.CloudSettings?.ShouldCallCloudService(request.GameTypeId) == true)
            {
                var voidGameRemoteApi = (IVoidGameRemoteApi)serviceProvider.GetService(typeof(IVoidGameRemoteApi));
                var header = await IdentityServerManager.GetAuthorizationHeader();

                return await voidGameRemoteApi.VoidTicket(request, header);
            }

            return await mediator.Send(request);
        }
    }
}