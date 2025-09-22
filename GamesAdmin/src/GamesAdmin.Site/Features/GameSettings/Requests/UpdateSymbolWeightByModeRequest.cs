using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSymbolWeightByModeRequest : IRequest<bool>
    {
        public UpdateSymbolWeightByModeRequest(
            string name,
            byte mode,
            int[][] weights,
            IEnumerable<int> otherGameIdsInGroup)
        {
            Name = name;
            Mode = mode;
            Weights = weights;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; set; }

        public byte Mode { get; set; }

        public int[][] Weights { get; set; }

        public IEnumerable<int> OtherGameIdsInGroup { get; set; }
    }
}
