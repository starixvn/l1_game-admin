using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.ApiLogs.ViewModels
{
    public class ApiLogViewModel
    {
        public string PartnerId { get; set; }

        public byte LogType { get; set; }

        public DateTimeOffset FromDate { get; set; } = DateTime.UtcNow;

        public DateTimeOffset ToDate { get; set; } = DateTime.UtcNow.AddDays(1);

        public string ObCustId { get; set; }

        public long GameRoundId { get; set; }

        public int GameTypeId { get; set; }

        public string Username { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; }

        public IEnumerable<SelectListItem> LogTypes { get; set; }

        public string SiteId { get; set; }
    }
}