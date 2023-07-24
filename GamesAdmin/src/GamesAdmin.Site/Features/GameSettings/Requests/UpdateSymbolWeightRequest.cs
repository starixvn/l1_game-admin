using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSymbolWeightRequest : IRequest<bool>
    {
        public UpdateSymbolWeightRequest(
            string name,
            SymbolWeight symbolWeight)
        {
            Name = name;
            SymbolWeight = symbolWeight;
        }

        public string Name { get; set; }

        public SymbolWeight SymbolWeight { get; set; }
    }
}
