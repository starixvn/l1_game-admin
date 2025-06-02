using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetJackpotReportRequest : IRequest<JackpotReportResultViewModel>
    {
        public GetJackpotReportRequest(
            int gameId,
            DateTime fromDate,
            DateTime toDate,
            bool excludeUUS,
            bool excludeBot,
            bool convertToINH,
            string baseCurrency,
            string operatorId,
            string siteNames)
        {
            GameId = gameId;
            FromDate = fromDate;
            ToDate = toDate;
            ExcludeUUS = excludeUUS;
            ExcludeBot = excludeBot;
            ConvertToINH = convertToINH;
            BaseCurrency = baseCurrency;
            OperatorId = operatorId;
            SiteNames = string.IsNullOrWhiteSpace(siteNames) ? Enumerable.Empty<string>() : siteNames.Split(';').Select(x => x.Trim());
        }

        public int GameId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool ExcludeUUS { get; }

        public bool ExcludeBot { get; }

        public bool ConvertToINH { get; }

        public string BaseCurrency { get; set; }

        public string OperatorId { get; set; }

        public IEnumerable<string> SiteNames { get; set; }
    }
}