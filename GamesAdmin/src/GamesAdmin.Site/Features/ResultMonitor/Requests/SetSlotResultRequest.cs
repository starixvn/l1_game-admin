using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Site.Features.ResultMonitor.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ResultMonitor.Requests
{
    public class SetSlotResultRequest : IRequest<SettlementBoardViewModel>
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