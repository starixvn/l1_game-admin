using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetCascadingSlotBetHistoryRequest : IRequest<CascadingSlotBetHistoryViewModel>
    {
        public GetCascadingSlotBetHistoryRequest(long roundId, string nickname, bool excludeUUS, string game, bool convertToUSD)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
            Game = Enumeration.FromValue<Game>(game.ToLowerInvariant());
            ConvertToUSD = convertToUSD;
        }


        public long RoundId { get; }

        public string Nickname { get; }

        public bool ConvertToUSD { get; }

        public bool ExcludeUUS { get; }

        public Game Game { get; }
    }
}
