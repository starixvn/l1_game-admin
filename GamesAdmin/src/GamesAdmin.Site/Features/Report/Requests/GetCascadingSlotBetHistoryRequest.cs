using System;
using System.Collections.Generic;using GamesAdmin.Site.Features.Report.ViewModels;
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
            Game = game;
            ConvertToUSD = convertToUSD;
        }


        public long RoundId { get; }

        public string Nickname { get; }

        public bool ConvertToUSD { get; }

        public bool ExcludeUUS { get; }

        public string Game { get; }
    }
}
