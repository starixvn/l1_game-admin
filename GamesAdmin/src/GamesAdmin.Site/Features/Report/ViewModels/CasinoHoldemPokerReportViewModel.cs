using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CasinoHoldemPokerReportViewModel : ReportViewModel
    {
        [CasinoHldemPokerValidationAttribute]
        public string Nickname { get; set; }

        public bool ExcludeUUS { get; set; }

        [DisplayName("Convert to USD")]
        public bool ConvertToUSD { get; set; }

        [DisplayName("Partner")]
        public string PartnerId { get; set; }

        [CasinoHldemPokerValidationAttribute]
        public string Username { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;

        public ReportSeletionsViewModel Selections { get; set; }

        //public CasinoHoldemPokerReportViewModel()
        //{
        //    Partner = new PartnerViewModel { PartnerId = "ONEworks1", PartnerName = "2W" };
        //    Partners = new List<PartnerViewModel>
        //    {
        //       Partner,
        //         new PartnerViewModel{PartnerId= "sbogamesimulator", PartnerName="BBO"},
        //    };

        //}

        //public PartnerViewModel Partner { get; set; }

        //public IEnumerable<PartnerViewModel> Partners { get; set; }

        //public DateTime FromDate { get; set; } = DateTime.Now;

        //public DateTime ToDate { get; set; } = DateTime.Now;

        //public string Username { get; set; }

        //public bool ConvertToUsd { get; set; }
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
            "Please input either Nickname or Username.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (CasinoHoldemPokerReportViewModel)validationContext.ObjectInstance;

            if (!string.IsNullOrWhiteSpace(model.Nickname) && !string.IsNullOrWhiteSpace(model.Username))
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