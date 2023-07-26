using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSymbolWeightByModeRequest : IRequest<bool>
    {
        public UpdateSymbolWeightByModeRequest(
            string name,
            byte mode,
            int[][] weights)
        {
            Name = name;
            Mode = mode;
            Weights = weights;
        }

        public string Name { get; set; }

        public byte Mode { get; set; }

        public int[][] Weights { get; set; }
    }
}
