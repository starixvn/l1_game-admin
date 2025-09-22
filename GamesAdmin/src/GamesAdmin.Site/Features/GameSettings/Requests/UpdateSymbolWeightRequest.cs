using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSymbolWeightRequest : IRequest<bool>
    {
        public UpdateSymbolWeightRequest(
            string name,
            SymbolWeight symbolWeight,
            IEnumerable<int> otherGameIdsInGroup)
        {
            Name = name;
            SymbolWeight = symbolWeight;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; set; }

        public SymbolWeight SymbolWeight { get; set; }

        public IEnumerable<int> OtherGameIdsInGroup { get; set; }
    }
}
