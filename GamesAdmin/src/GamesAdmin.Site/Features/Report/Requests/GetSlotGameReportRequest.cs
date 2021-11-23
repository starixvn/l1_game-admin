using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using System;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetSlotGameReportRequest : IRequest<SlotGameReportResultViewModel>
    {
        public GetSlotGameReportRequest(
            long roundId,
            string nickname,
            bool excludeUUS,
            string game,
            bool convertToUSD)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
            Game = Enumeration.FromValue<Game>(game);
            ConvertToUSD = convertToUSD;
        }

        public GetSlotGameReportRequest(
            long roundId,
            string nickname,
            bool excludeUUS,
            string game,
            string partnerId,
            DateTime fromDate,
            DateTime toDate,
            string username,
            bool convertToUSD) : this(roundId, nickname, excludeUUS, game, convertToUSD)
        {
            PartnerId = partnerId;
            FromDate = fromDate;
            ToDate = toDate;
            Username = username;
        }

        public string PartnerId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public long RoundId { get; }

        public string Nickname { get; }

        public string Username { get; }

        public bool ConvertToUSD { get; }

        public bool ExcludeUUS { get; }

        public Game Game { get; }
    }
}