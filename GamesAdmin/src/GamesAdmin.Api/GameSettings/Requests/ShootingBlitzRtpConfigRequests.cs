using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class GetAllShootingBlitzRtpConfigRequest : IRequest<IEnumerable<ShootingBlitzRtpConfig>>
    {
    }

    public class GetShootingBlitzRtpConfigByIdRequest : IRequest<ShootingBlitzRtpConfig>
    {
        public GetShootingBlitzRtpConfigByIdRequest(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }

    public class CreateShootingBlitzRtpConfigRequest : IRequest<ShootingBlitzRtpConfig>
    {
        public double Rtp { get; set; }

        public List<BombCountConfig> BombCount { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    public class UpdateShootingBlitzRtpConfigRequest : IRequest<bool>
    {
        public string Id { get; set; }

        public double Rtp { get; set; }

        public List<BombCountConfig> BombCount { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    public class DeleteShootingBlitzRtpConfigRequest : IRequest<bool>
    {
        public DeleteShootingBlitzRtpConfigRequest(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

