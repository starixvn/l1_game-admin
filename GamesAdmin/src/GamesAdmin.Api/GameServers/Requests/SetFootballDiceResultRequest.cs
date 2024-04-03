using GamesAdmin.Core.Models.FootballDice;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetFootballDiceResultRequest : IRequest<FootballDiceControlResult>
    {
        public SetFootballDiceResultRequest(FootballDiceResult result)
        {
            this.result = result;
        }

        public FootballDiceResult result { get; set; }
    }

    public class FootballDiceResult
    {
        public double[] home { get; set; }

        public double[] away { get; set; }
    }
}