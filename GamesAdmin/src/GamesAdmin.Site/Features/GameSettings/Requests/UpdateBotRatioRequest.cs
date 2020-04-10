using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateBotRatioRequest : IRequest<bool>
    {
        public UpdateBotRatioRequest(string name, double[] botRatios) 
        {
            Name = name;
            BotRatios = botRatios;
        }

        public string Name { get; }

        public double[] BotRatios { get; }
    }
}
