using System;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetCascadingSlotReportRequest : IRequest<CascadingSlotReportResultViewModel>
    {
        public GetCascadingSlotReportRequest(
            long roundId,
            string nickname,
            bool excludeUUS,
            string game,
            bool convertToUSD)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
            Game = game;
            ConvertToUSD = convertToUSD;
        }

        public GetCascadingSlotReportRequest(
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

        public string Game { get; }
    }
}