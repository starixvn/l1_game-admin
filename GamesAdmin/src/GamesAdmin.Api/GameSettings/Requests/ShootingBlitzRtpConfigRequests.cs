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

    /// <summary>
    /// [Deprecated] Use UpdateShootingBlitzConfigRequest with ActionType.CreateRtpConfig instead
    /// </summary>
    public class CreateShootingBlitzRtpConfigRequest : IRequest<ShootingBlitzRtpConfig>
    {
        /// <summary>
        /// RTP Config 顯示名稱 (e.g., "RTP 95% - Normal Mode")
        /// </summary>
        public string Name { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    /// <summary>
    /// [Deprecated] Use UpdateShootingBlitzConfigRequest with ActionType.UpdateRtpConfig instead
    /// </summary>
    public class UpdateShootingBlitzRtpConfigSingleRequest : IRequest<bool>
    {
        public string Id { get; set; }

        /// <summary>
        /// RTP Config 顯示名稱 (e.g., "RTP 95% - Normal Mode")
        /// </summary>
        public string Name { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    /// <summary>
    /// [Deprecated] Use UpdateShootingBlitzConfigRequest with ActionType.DeleteRtpConfig instead
    /// </summary>
    public class DeleteShootingBlitzRtpConfigRequest : IRequest<bool>
    {
        public DeleteShootingBlitzRtpConfigRequest(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

