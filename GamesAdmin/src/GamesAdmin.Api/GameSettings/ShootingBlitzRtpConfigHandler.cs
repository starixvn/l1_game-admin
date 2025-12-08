using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings
{
    /// <summary>
    /// [Deprecated] Use GameSettingsHandler with UpdateShootingBlitzConfigRequest instead.
    /// This handler is kept for backward compatibility.
    /// </summary>
    public class ShootingBlitzRtpConfigHandler
        : IRequestHandler<GetAllShootingBlitzRtpConfigRequest, IEnumerable<ShootingBlitzRtpConfig>>
        , IRequestHandler<GetShootingBlitzRtpConfigByIdRequest, ShootingBlitzRtpConfig>
        , IRequestHandler<CreateShootingBlitzRtpConfigRequest, ShootingBlitzRtpConfig>
        , IRequestHandler<UpdateShootingBlitzRtpConfigSingleRequest, bool>
        , IRequestHandler<DeleteShootingBlitzRtpConfigRequest, bool>
    {
        private readonly IShootingBlitzRtpConfigService service;

        public ShootingBlitzRtpConfigHandler(IShootingBlitzRtpConfigService service)
        {
            this.service = service;
        }

        public Task<IEnumerable<ShootingBlitzRtpConfig>> Handle(GetAllShootingBlitzRtpConfigRequest request, CancellationToken cancellationToken)
            => service.GetAllAsync();

        public Task<ShootingBlitzRtpConfig> Handle(GetShootingBlitzRtpConfigByIdRequest request, CancellationToken cancellationToken)
            => service.GetByIdAsync(request.Id);

        public Task<ShootingBlitzRtpConfig> Handle(CreateShootingBlitzRtpConfigRequest request, CancellationToken cancellationToken)
        {
            var config = new ShootingBlitzRtpConfig
            {
                Name = request.Name,
                MultiplierSteps = request.MultiplierSteps
            };
            return service.CreateAsync(config);
        }

        public Task<bool> Handle(UpdateShootingBlitzRtpConfigSingleRequest request, CancellationToken cancellationToken)
        {
            var config = new ShootingBlitzRtpConfig
            {
                Id = request.Id,
                Name = request.Name,
                MultiplierSteps = request.MultiplierSteps
            };
            return service.UpdateAsync(request.Id, config);
        }

        public Task<bool> Handle(DeleteShootingBlitzRtpConfigRequest request, CancellationToken cancellationToken)
            => service.DeleteAsync(request.Id);
    }
}

