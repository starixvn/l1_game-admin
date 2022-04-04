using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Site.Features.ResultMonitor.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ResultMonitor.Requests
{
    public class SetCascadingSlotResultRequest : IRequest<CascadingSlotBoardViewModel>
    {
        public SetCascadingSlotResultRequest(
            string gameConfigName,
            string nickname,
            string baseSpin,
            List<string> tumblings)
        {
            GameConfigName = gameConfigName;
            Nickname = nickname;
            BaseSpin = ConvertResult(baseSpin);
            Tumblings = tumblings.Select(t => ConvertResult(t));
        }

        public string GameConfigName { get; }

        public string Nickname { get; }

        public byte[] BaseSpin { get; }

        public IEnumerable<byte[]> Tumblings { get; }

        private byte[] ConvertResult(string result)
            => result.Trim().Split(',').Select(x => byte.Parse(x)).ToArray();
    }
}