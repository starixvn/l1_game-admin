namespace GamesAdmin.Site.Features.DailySummary.Models
{
    using System;
    using GamesAdmin.Core.Models.ApiLogs;
    using GamesAdmin.Site.Features.DailySummary.ViewModels;

    public class DailySummaryRequest
    {
        public DailySummaryRequest(DailySummaryViewModel model)
        {
            Auth = new Auth
            {
                OperatorId = model.OperatorId,
                OperatorSecret = model.OperatorSecret
            };
            SummarizedDate = model.Date;
            Seq = Guid.NewGuid().ToString();
            SiteId = model.SiteId;
        }

        public Auth Auth { get; set; }

        public DateTimeOffset SummarizedDate { get; set; }

        public string Seq { get; set; }

        public string SiteId { get; set; }
    }
}
