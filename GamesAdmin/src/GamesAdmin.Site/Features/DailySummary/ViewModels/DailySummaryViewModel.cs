namespace GamesAdmin.Site.Features.DailySummary.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GamesAdmin.Core.Models.Partners;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class DailySummaryViewModel
    {
        public IEnumerable<Partner> Operators { get; set; } = new List<Partner>();

        public IEnumerable<SelectListItem> OperatorItem => Operators.Select(o => new SelectListItem(o.PartnerId, o.PartnerId));

        public DateTimeOffset Date { get; set; } = DateTime.UtcNow;

        public string OperatorId { get; set; }

        public string OperatorSecret { get; set; }
    }
}
