using GamesAdmin.Api.UM.Requests;
using GamesAdmin.Core.Models.Um;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.UM
{
    public class UMHandler
        : IRequestHandler<UMRequest, bool>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<GetAllRequest, IList<UmSchedule>>
    {
        private readonly IUMService umService;

        public UMHandler(IUMService umService) 
        {
            this.umService = umService;
        }

        public Task<bool> Handle(UMRequest request, CancellationToken cancellationToken)
        => umService.Process(request.IsUM, request.StartTime, request.EndTime);

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => umService.AddSchedule(request.EventSchedule);

        public Task<IList<UmSchedule>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => umService.GetAll();
    }
}
