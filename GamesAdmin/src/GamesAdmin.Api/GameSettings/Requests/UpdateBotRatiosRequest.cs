using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateBotRatiosRequest : IRequest<bool>
    {
        public UpdateBotRatiosRequest(string name, double[] ratios)
        {
            Name = name;
            Ratios = ratios;
        }

        public string Name { get; }

        public double[] Ratios { get; }
    }
}