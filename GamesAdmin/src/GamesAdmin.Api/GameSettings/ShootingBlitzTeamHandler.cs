using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings
{
    public class ShootingBlitzTeamHandler
        : IRequestHandler<GetAllShootingBlitzTeamRequest, IEnumerable<ShootingBlitzTeam>>
        , IRequestHandler<GetShootingBlitzTeamByIdRequest, ShootingBlitzTeam>
        , IRequestHandler<CreateShootingBlitzTeamRequest, ShootingBlitzTeam>
        , IRequestHandler<UpdateShootingBlitzTeamRequest, bool>
        , IRequestHandler<DeleteShootingBlitzTeamRequest, bool>
    {
        private readonly IShootingBlitzTeamService service;

        public ShootingBlitzTeamHandler(IShootingBlitzTeamService service)
        {
            this.service = service;
        }

        public Task<IEnumerable<ShootingBlitzTeam>> Handle(GetAllShootingBlitzTeamRequest request, CancellationToken cancellationToken)
            => service.GetAllAsync();

        public Task<ShootingBlitzTeam> Handle(GetShootingBlitzTeamByIdRequest request, CancellationToken cancellationToken)
            => service.GetByIdAsync(request.Id);

        public Task<ShootingBlitzTeam> Handle(CreateShootingBlitzTeamRequest request, CancellationToken cancellationToken)
        {
            var team = new ShootingBlitzTeam
            {
                TeamId = request.TeamId,
                Name = request.Name,
                StarPlayList = request.StarPlayList
            };
            return service.CreateAsync(team);
        }

        public Task<bool> Handle(UpdateShootingBlitzTeamRequest request, CancellationToken cancellationToken)
        {
            var team = new ShootingBlitzTeam
            {
                Id = request.Id,
                TeamId = request.TeamId,
                Name = request.Name,
                StarPlayList = request.StarPlayList
            };
            return service.UpdateAsync(request.Id, team);
        }

        public Task<bool> Handle(DeleteShootingBlitzTeamRequest request, CancellationToken cancellationToken)
            => service.DeleteAsync(request.Id);
    }
}

