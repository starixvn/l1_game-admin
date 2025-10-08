using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GamesAdmin.Site.Features.BetLimitSuggestion.ViewModels
{
  public class GetBetLimitSuggestionModel
  {
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Currency rate must be greater than 0")]
    public double CurrencyRate { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one candidate currency group ID is required")]
    public IEnumerable<string> CandidateCurrencyGroupIds { get; set; } = new List<string>();

    [Required]
    [StringLength(50, ErrorMessage = "Base currency group ID cannot exceed 50 characters")]
    public string BaseCurrencyGroupId { get; set; }
  }
}
