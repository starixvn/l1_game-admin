using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateOddsByCountsRequest : IRequest<bool>
    {
        public UpdateOddsByCountsRequest(string name, SlotsConfig slotsConfig)
        {
            Name = name;
            SlotsConfig = slotsConfig;
        }

        public string Name { get; }

        public SlotsConfig SlotsConfig { get; }
    }
}