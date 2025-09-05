using System;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameSettings.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GameIdRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addViewModel = validationContext.ObjectInstance as ViewModels.AddViewModel;

            if (addViewModel?.Type == GameType.AutoRNGIndex && (value == null || !(value is int gameId) || gameId <= 0))
            {
                return new ValidationResult("Game ID is required and must be a positive number for AutoRNGIndex games");
            }

            return ValidationResult.Success;
        }
    }
}
