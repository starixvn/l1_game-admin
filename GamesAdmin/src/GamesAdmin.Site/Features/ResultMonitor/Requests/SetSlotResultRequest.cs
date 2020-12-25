using GamesAdmin.Core.Models.SlotGame;
using MediatR;

namespace GamesAdmin.Site.Features.ResultMonitor.Requests
{
    public class SetSlotResultRequest : IRequest<SettlementBoard>
    {
        public SetSlotResultRequest(string gameConfigName, string nickname, byte[] results)
        {
            GameConfigName = gameConfigName;
            Nickname = nickname;
            Results = results;
        }

        public string GameConfigName { get; }

        public string Nickname { get; }

        public byte[] Results { get; }
    }
}