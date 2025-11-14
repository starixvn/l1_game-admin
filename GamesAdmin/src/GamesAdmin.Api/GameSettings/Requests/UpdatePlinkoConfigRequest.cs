using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;
using System;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdatePlinkoConfigRequest : IRequest<bool>
    {
        public UpdatePlinkoConfigRequest() { }

        public UpdatePlinkoConfigRequest(
            string name,
            PlinkoConfig plinkoConfig)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (plinkoConfig == null)
            {
                throw new ArgumentNullException(nameof(plinkoConfig));
            }

            Name = name;
            PlinkoConfig = plinkoConfig;
        }

        public string Name { get; set; }

        public PlinkoConfig PlinkoConfig { get; set; }
    }
}
