using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdatePlinkoConfigRequest : IRequest<bool>
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
