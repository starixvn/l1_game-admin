using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ReportSeletionsViewModel
    {
        public IEnumerable<SelectListItem> Partners { get; set; }

        public IEnumerable<SelectListItem> Currencies { get; set; }
    }

    public class StakeBalancedViewModel : ReportViewModel
    {
        [DisplayName("Convert to Currency")]
        public string Currency { get; set; }
    }

    public class ReportViewModel
    {
        [Required(ErrorMessage = "Please input RoundId.")]
        public virtual long? RoundId { get; set; }

        public string Nickname { get; set; }

        [DisplayName("Exclude Bots")]
        public bool ExcludeBot { get; set; }

        [DisplayName("Exclude UUS")]
        public bool ExcludeUUS { get; set; }

        public GameId Game { get; set; }

        public ReportSeletionsViewModel Selections { get; set; }

        [DisplayName("Partner")]
        public string PartnerId { get; set; }

        public string Partner { get; set; }

        public string Username { get; set; }

        public bool ConvertToUSD { get; set; }

        public string SystemType { get; set; }

        public string BaseCurrency { get; set; }

        public IEnumerable<SelectListItem> Games { get; set; }
    }

    public class JackpotReportViewModel
    {
        public byte GameId { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Now;

        public DateTime ToDate { get; set; } = DateTime.Now;

        public bool ExcludeBot { get; set; } = true;

        public bool ExcludeUUS { get; set; } = true;
    }

    public class KenoMatchReportViewModel : ReportViewModel
    {
    }

    public class KenoPowerReportViewModel : ReportViewModel
    {
    }
}