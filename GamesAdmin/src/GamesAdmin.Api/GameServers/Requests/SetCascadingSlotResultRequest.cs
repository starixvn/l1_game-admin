using System.Collections.Generic;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetCascadingSlotResultRequest : IRequest<CascadingSettlementResult>
    {
        public SetCascadingSlotResultRequest(
            string gameConfigName,
            string nickname,
            byte[] baseSpin,
            IEnumerable<byte[]> tumblings)
        {
            GameConfigName = gameConfigName;
            Nickname = nickname;
            BaseSpin = baseSpin;
            Tumblings = tumblings;
        }

        public string GameConfigName { get; }

        public string Nickname { get; }

        public byte[] BaseSpin { get; }

        public IEnumerable<byte[]> Tumblings { get; }
    }
}