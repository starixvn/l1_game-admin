using System;
using GamesAdmin.Site.Features.ApiLogs.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ApiLogs.Requests
{
    public class QueryApiLogRequest : IRequest<ApiLogListViewModel>
    {
        public string PartnerId { get; set; }

        public byte LogType { get; set; }

        public DateTimeOffset FromDate { get; set; }

        public DateTimeOffset ToDate { get; set; }

        public string ObCustId { get; set; }

        public long GameRoundId { get; set; }

        public int GameTypeId { get; set; }

        public string Username { get; set; }

        public string SiteId { get; set; }
    }
}