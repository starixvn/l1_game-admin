using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class CreateCurrencyWithSuggestionViewModel : CreateCurrencyViewModel
    {
        public bool GetFromAllOperators { get; set; }
        
        [Required(ErrorMessage = "The Rate to Base is required field.")]
        public double RateToBase { get; set; }
        
        public IEnumerable<SelectListItem> CurrencyGroups { get; set; }
        
        public List<CurrencyGroupSuggestionModel> Suggestions { get; set; } = new List<CurrencyGroupSuggestionModel>();
        
        public bool ShowSuggestions { get; set; } = false;
    }
    
    public class CurrencyGroupSuggestionModel
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string PartnerId { get; set; }
        public List<GameBetLimitModel> Games { get; set; } = new List<GameBetLimitModel>();
    }
    
    public class GameBetLimitModel
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public double MinBet { get; set; }
        public double MaxBet { get; set; }
        public double[] BetLevels { get; set; }
        public double[] CoinValues { get; set; }
    }
} 