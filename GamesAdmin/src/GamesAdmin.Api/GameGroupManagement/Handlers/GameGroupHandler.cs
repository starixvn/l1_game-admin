using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameGroupManagement.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameGroupManagement.Handlers
{
    public class GameGroupHandler :
        IRequestHandler<CreateRequest, bool>,
        IRequestHandler<UpdateRequest, bool>,
        IRequestHandler<GetAllRequest, IEnumerable<GameGroup>>,
        IRequestHandler<GetByGameIdRequest, GameGroup>,
        IRequestHandler<GetByGroupIdRequest, GameGroup>
    {
        private readonly IGameGroupService gameGroupService;

        public GameGroupHandler(IGameGroupService gameGroupService)
        {
            this.gameGroupService = gameGroupService;
        }

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            return gameGroupService.Create(request.GameGroup);
        }

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            return gameGroupService.Update(request.GameGroup);
        }

        public Task<IEnumerable<GameGroup>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            return gameGroupService.GetAll();
        }

        public Task<GameGroup> Handle(GetByGameIdRequest request, CancellationToken cancellationToken)
        {
            return gameGroupService.GetByGameId(request.GameId);
        }

        public Task<GameGroup> Handle(GetByGroupIdRequest request, CancellationToken cancellationToken)
        {
            return gameGroupService.GetByGroupId(request.GroupId);
        }
    }
}