using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings
{
    public class ShootingBlitzRtpConfigHandler
        : IRequestHandler<GetAllShootingBlitzRtpConfigRequest, IEnumerable<ShootingBlitzRtpConfig>>
        , IRequestHandler<GetShootingBlitzRtpConfigByIdRequest, ShootingBlitzRtpConfig>
        , IRequestHandler<CreateShootingBlitzRtpConfigRequest, ShootingBlitzRtpConfig>
        , IRequestHandler<UpdateShootingBlitzRtpConfigRequest, bool>
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
                Rtp = request.Rtp,
                BombCount = request.BombCount,
                MultiplierSteps = request.MultiplierSteps
            };
            return service.CreateAsync(config);
        }

        public Task<bool> Handle(UpdateShootingBlitzRtpConfigRequest request, CancellationToken cancellationToken)
        {
            var config = new ShootingBlitzRtpConfig
            {
                Id = request.Id,
                Rtp = request.Rtp,
                BombCount = request.BombCount,
                MultiplierSteps = request.MultiplierSteps
            };
            return service.UpdateAsync(request.Id, config);
        }

        public Task<bool> Handle(DeleteShootingBlitzRtpConfigRequest request, CancellationToken cancellationToken)
            => service.DeleteAsync(request.Id);
    }
}

