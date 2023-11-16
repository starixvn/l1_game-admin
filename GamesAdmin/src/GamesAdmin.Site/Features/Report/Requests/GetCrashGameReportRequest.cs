using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using System;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetCrashGameReportRequest : IRequest<CrashGameReportResultViewModel>
    {
        public GetCrashGameReportRequest(
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

        public GetCrashGameReportRequest(
            long roundId,
            string nickname,
            bool excludeUUS,
            string game,
            string partnerId,
            string username,
            bool convertToUSD) : this(roundId, nickname, excludeUUS, game, convertToUSD)
        {
            PartnerId = partnerId;
            Username = username;
        }

        public string PartnerId { get; }


        public long RoundId { get; }

        public string Nickname { get; }

        public string Username { get; }

        public bool ConvertToUSD { get; }

        public bool ExcludeUUS { get; }

        public string Game { get; }
    }
}
