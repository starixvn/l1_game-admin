using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Site.Features.ProVietKeno.ViewModels
{
    public class CreateProVietKenoRoundViewModel : IValidatableObject
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long? RoundNo { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Today;

        public bool[] Result { get; set; } = new bool[80];

        public  IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Result.Count(x => x) != 20)
            {
                yield return new ValidationResult("Please select 20 numbers", new[] { nameof(Result) });
            }
        }
    }
}