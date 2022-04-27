using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateOddsByCountsRequest : IRequest<bool>
    {
        public UpdateOddsByCountsRequest(string name, CascadingSlotsConfig cascadingSlotsConfig)
        {
            Name = name;
            CascadingSlotsConfig = cascadingSlotsConfig;
        }

        public string Name { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }
    }
}