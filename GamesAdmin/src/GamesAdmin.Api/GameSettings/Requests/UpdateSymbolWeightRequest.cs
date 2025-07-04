using System.Collections;
using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateSymbolWeightRequest : IRequest<bool>
    {
        public UpdateSymbolWeightRequest(string name, SymbolWeight symbolWeight, IEnumerable<int> otherGameIdsInGroup)
        {
            Name = name;
            SymbolWeight = symbolWeight;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }
        public string Name { get; }

        public SymbolWeight SymbolWeight { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}
