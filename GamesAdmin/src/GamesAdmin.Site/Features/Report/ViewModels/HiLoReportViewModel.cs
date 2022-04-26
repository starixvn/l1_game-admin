using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HiLoReportViewModel
    {
        [HiLoValidation]
        public string Nickname { get; set; }
        public bool ExcludeUUS { get; set; }

        [DisplayName("Convert to INH")]
        public bool ConvertToUSD { get; set; }

        [DisplayName("Partner")]
        public string PartnerId { get; set; }

        [HiLoValidation]
        public string Username { get; set; }

        [HiLoValidation]
        public long? RoundId { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;

        public ReportSeletionsViewModel Selections { get; set; }
    }



    [AttributeUsage(AttributeTargets.Property)]
    public class HiLoValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-eitherrequired", GetErrorMessage());
        }

        public string GetErrorMessage() =>
            "Please input either Round Id, Nickname or Username.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (HiLoReportViewModel)validationContext.ObjectInstance;

            if (!model.RoundId.HasValue || !string.IsNullOrWhiteSpace(model.Nickname) && !string.IsNullOrWhiteSpace(model.Username))
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