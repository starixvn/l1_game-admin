using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BlackjackReportViewModel
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
        public long? HandId { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;

        public ReportSeletionsViewModel Selections { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class BlalckjackValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-eitherrequired", GetErrorMessage());
        }

        public string GetErrorMessage() =>
            "Please input either HandId, Nickname or Username.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (BlackjackReportViewModel)validationContext.ObjectInstance;

            if (!model.HandId.HasValue || !string.IsNullOrWhiteSpace(model.Nickname) && !string.IsNullOrWhiteSpace(model.Username))
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