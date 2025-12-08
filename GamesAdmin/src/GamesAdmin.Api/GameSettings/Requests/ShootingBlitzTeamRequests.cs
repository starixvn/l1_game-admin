using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class GetAllShootingBlitzTeamRequest : IRequest<IEnumerable<ShootingBlitzTeam>>
    {
    }

    public class GetShootingBlitzTeamByIdRequest : IRequest<ShootingBlitzTeam>
    {
        public GetShootingBlitzTeamByIdRequest(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }

    public class CreateShootingBlitzTeamRequest : IRequest<ShootingBlitzTeam>
    {
        public int TeamId { get; set; }

        public string Name { get; set; }

        public List<ShootingBlitzPlayer> StarPlayList { get; set; }
    }

    public class UpdateShootingBlitzTeamRequest : IRequest<bool>
    {
        public string Id { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; }

        public List<ShootingBlitzPlayer> StarPlayList { get; set; }
    }

    public class DeleteShootingBlitzTeamRequest : IRequest<bool>
    {
        public DeleteShootingBlitzTeamRequest(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

