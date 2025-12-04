using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateShootingBlitzRtpSiteRequest : IRequest<ShootingBlitzRtpConfig>
    {
        public double Rtp { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    public class UpdateShootingBlitzRtpSiteRequest : IRequest<bool>
    {
        public string Id { get; set; }

        public double Rtp { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    public class DeleteShootingBlitzRtpSiteRequest : IRequest<bool>
    {
        public string Id { get; set; }
    }

    public class SelectShootingBlitzRtpSiteRequest : IRequest<bool>
    {
        public string Name { get; set; }

        public string RtpConfigId { get; set; }
    }
}

