using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameServers;
using GamesAdmin.Api.UM.Requests;
using GamesAdmin.Core.Models.Um;
using MediatR;

namespace GamesAdmin.Api.UM
{
    public class UMHandler
        : IRequestHandler<UMRequest, bool>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<GetAllRequest, IList<UmSchedule>>
        , IRequestHandler<GetUnfinishedUmRequest, IList<UmSchedule>>
        , IRequestHandler<GetUMInfoRequest, IEnumerable<UmInfo>>
        , IRequestHandler<UpdatePartnerUMRequest, bool>
    {
        private readonly IUMService umService;
        private readonly IGameServerService gameServerService;

        public UMHandler(IUMService umService, IGameServerService gameServerService)
        {
            this.umService = umService;
            this.gameServerService = gameServerService;
        }

        public async Task<bool> Handle(UMRequest request, CancellationToken cancellationToken)
        {
            await gameServerService.UnderMaintenance(request);

            return await umService.Process(request.IsUM, request.StartTime, request.EndTime);
        }

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => umService.AddSchedule(request.EventSchedule);

        public Task<IList<UmSchedule>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => umService.GetAll();

        public Task<IList<UmSchedule>> Handle(GetUnfinishedUmRequest request, CancellationToken cancellationToken)
        => umService.GetUnfinished();

        public Task<IEnumerable<UmInfo>> Handle(GetUMInfoRequest request, CancellationToken cancellationToken)
        => umService.GetUMInfo();

        public Task<bool> Handle(UpdatePartnerUMRequest request, CancellationToken cancellationToken)
        => umService.UpdatePartnerUM(request.IsUM, request.StartTime, request.EndTime, request.Partners);
    }
}