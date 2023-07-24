using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateSymbolWeightRequest : IRequest<bool>
    {
        public UpdateSymbolWeightRequest(string name, SymbolWeight symbolWeight)
        {
            Name = name;
            SymbolWeight = symbolWeight;
        }
        public string Name { get; }

        public SymbolWeight SymbolWeight { get; }
    }
}
