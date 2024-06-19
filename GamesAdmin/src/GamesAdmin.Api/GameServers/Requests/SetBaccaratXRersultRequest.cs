using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetBaccaratXRersultRequest : IRequest<bool>
    {
        public SetBaccaratXRersultRequest(string gameConfigName, BaccaratXResult result)
        {
            this.gameConfigName = gameConfigName;
            this.result = result;
        }

        public string gameConfigName { get; set; }

        public BaccaratXResult result { get; set; }
    }

    public class BaccaratXResult
    {
        public PokerCard[] cards { get; set; }

        public byte cutCardIndex { get; set; }

        public LuckyCard[] luckyCards { get; set; }
    }

    public class LuckyCard : PokerCard
    {
        public byte value { get; set; }
    }
}
