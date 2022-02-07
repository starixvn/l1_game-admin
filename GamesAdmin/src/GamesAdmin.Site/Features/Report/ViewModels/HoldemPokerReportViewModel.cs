using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HoldemPokerReportViewModel
    {
        [CasinoHldemPokerValidation]
        public long? RoundId { get; set; }

        [DisplayName("From Date")]
        public DateTime FromDate { get; set; } = DateTime.Today;

        [DisplayName("To Date")]
        public DateTime ToDate { get; set; } = DateTime.Today;

        [CasinoHldemPokerValidation]
        public string Nickname { get; set; }

        [CasinoHldemPokerValidation]
        public string Username { get; set; }

        [DisplayName("Convert to USD")]
        public bool ConvertToUSD { get; set; }

        [DisplayName("Partner")]
        public string PartnerId { get; set; }

        public bool ExcludeUUS { get; set; }

        public ReportSeletionsViewModel Selections { get; set; }

        public Game Game { get; set; }
    }

    public class PartnerViewModel
    {
        public string PartnerId { get; set; }

        public string PartnerName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CasinoHldemPokerValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-eitherrequired", GetErrorMessage());
        }

        public string GetErrorMessage() =>
             "Please input either RoundId, Nickname or Username.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (HoldemPokerReportViewModel)validationContext.ObjectInstance;

            if (!model.RoundId.HasValue && !string.IsNullOrWhiteSpace(model.Nickname) && !string.IsNullOrWhiteSpace(model.Username))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}
