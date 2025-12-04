using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IShootingBlitzRtpApi : IBaseAuthorizationApi
    {
        [Get("/shooting_blitz_rtp")]
        Task<IList<ShootingBlitzRtpConfig>> GetAll();

        [Get("/shooting_blitz_rtp/{id}")]
        Task<ShootingBlitzRtpConfig> GetById(string id);

        [Post("/shooting_blitz_rtp")]
        Task<ShootingBlitzRtpConfig> Create([Body] CreateShootingBlitzRtpApiRequest request);

        [Put("/shooting_blitz_rtp/{id}")]
        Task<bool> Update(string id, [Body] UpdateShootingBlitzRtpApiRequest request);

        [Delete("/shooting_blitz_rtp/{id}")]
        Task<bool> Delete(string id);

        [Post("/shooting_blitz_rtp/select")]
        Task<bool> Select([Body] SelectShootingBlitzRtpConfigRequest request);
    }

    public class CreateShootingBlitzRtpApiRequest
    {
        public double Rtp { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    public class UpdateShootingBlitzRtpApiRequest
    {
        public double Rtp { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    public class SelectShootingBlitzRtpConfigRequest
    {
        public string Name { get; set; }

        public string RtpConfigId { get; set; }
    }
}

