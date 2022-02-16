using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class BitcoinMasterReportRequest : IRequest<BitcoinMasterBetReport>
    {
        public BitcoinMasterReportRequest(
            long roundId,
            string nickname,
            bool excludeBot,
            string partnerId,
            string username)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeBot = excludeBot;
            PartnerId = partnerId;
            Username = username;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeBot { get; }

        public string PartnerId { get; }

        public string Username { get; }
    }
}