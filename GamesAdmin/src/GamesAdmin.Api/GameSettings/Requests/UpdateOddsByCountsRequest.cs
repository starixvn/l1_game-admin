using System.Collections;
using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateOddsByCountsRequest : IRequest<bool>
    {
        public UpdateOddsByCountsRequest(string name, CascadingSlotsConfig cascadingSlotsConfig, IEnumerable<int> otherGameIdsInGroup = null)
        {
            Name = name;
            CascadingSlotsConfig = cascadingSlotsConfig;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}