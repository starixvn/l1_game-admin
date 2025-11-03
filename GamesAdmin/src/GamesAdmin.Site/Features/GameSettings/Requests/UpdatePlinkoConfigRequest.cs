using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdatePlinkoConfigRequest : IRequest<string>
    {
        public UpdatePlinkoConfigRequest() { }

        public UpdatePlinkoConfigRequest(
            string name,
            PlinkoConfig plinkoConfig)
        {
            Name = name;
            PlinkoConfig = plinkoConfig;
        }

        public string Name { get; set; }

        public PlinkoConfig PlinkoConfig { get; set; }
    }
}
