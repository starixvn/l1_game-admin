using GamesAdmin.Site.Features.Report.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Report.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SlotGameReportValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-eitherrequired", GetErrorMessage());
        }

        public string GetErrorMessage() => "Please input either RoundId, Nickname or Username.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (SlotGameReportViewModel)validationContext.ObjectInstance;

            var isValid = model.RoundId.HasValue ||
                !string.IsNullOrWhiteSpace(model.Nickname) ||
                !string.IsNullOrWhiteSpace(model.Username);

            return isValid ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
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
