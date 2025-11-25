using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameSettings.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GameIdRequiredAttribute : ValidationAttribute
    {
        private static readonly HashSet<GameType> GameTypesRequiringGameId = new HashSet<GameType>
        {
            GameType.AutoRNGIndex,
            GameType.MarbleProduct,
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addViewModel = validationContext.ObjectInstance as ViewModels.AddViewModel;

            if (addViewModel == null)
            {
                return ValidationResult.Success;
            }

            if (!GameTypesRequiringGameId.Contains(addViewModel.Type))
            {
                return ValidationResult.Success;
            }

            if (!IsValidGameId(value))
            {
                var gameTypeName = addViewModel.Type.ToString();
                return new ValidationResult(
                    $"Game ID is required and must be a positive number for {gameTypeName} games"
                );
            }

            return ValidationResult.Success;
        }

        private static bool IsValidGameId(object value)
        {
            if (value == null)
            {
                return false;
            }

            return value is int gameId && gameId > 0;
        }
    }
}
