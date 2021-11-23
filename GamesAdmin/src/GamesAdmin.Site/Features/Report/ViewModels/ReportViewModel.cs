using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class ReportSeletionsViewModel
    {
        public IEnumerable<SelectListItem> Partners { get; set; }

        public IEnumerable<SelectListItem> Currencies { get; set; }
    }

    public class StakeBalancedViewModel : ReportViewModel
    {
        [Required(ErrorMessage = "Please input RoundId.")]
        public override long? RoundId { get; set; }

        [DisplayName("Convert to Currency")]
        public string Currency { get; set; }
    }

    public class ReportViewModel
    {
        public virtual long? RoundId { get; set; }

        public string Nickname { get; set; }

        public bool ExcludeBot { get; set; }

        public bool ExcludeUUS { get; set; }

        public string Game { get; set; }

        public ReportSeletionsViewModel Selections { get; set; }

        public string Partner { get; set; }

        public string Username { get; set; }
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
        public GameId Game { get; set; }
    }

    public class KenoPowerReportViewModel : ReportViewModel
    {
        public GameId Game { get; set; }
    }
}