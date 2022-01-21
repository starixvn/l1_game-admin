using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HiLoReportViewModel
    {
        [BlalckjackValidation]
        public string Nickname { get; set; }

        [DisplayName("Exclude UUS")]
        public bool ExcludeUUS { get; set; }

        [DisplayName("Convert to USD")]
        public bool ConvertToUSD { get; set; }

        [DisplayName("Partner")]
        public string PartnerId { get; set; }

        [BlalckjackValidation]
        public string Username { get; set; }

        [BlalckjackValidation]
        public long? RoundId { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;

        public ReportSeletionsViewModel Selections { get; set; }
    }
}