using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetExistingCurrencyGroupSuggestionsHandler : Handler, IRequestHandler<GetExistingCurrencyGroupSuggestionsRequest, CreateCurrencyWithSuggestionViewModel>
    {
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly IGroupCurrencyGameApi currencyGroupGameApi;

        public GetExistingCurrencyGroupSuggestionsHandler(
            IMapper mapper, 
            ICurrencyApi api, 
            ICurrencyGroupService currencyGroupService,
            IGroupCurrencyGameApi currencyGroupGameApi) : base(mapper, api)
        {
            this.currencyGroupService = currencyGroupService;
            this.currencyGroupGameApi = currencyGroupGameApi;
        }

        public async Task<CreateCurrencyWithSuggestionViewModel> Handle(GetExistingCurrencyGroupSuggestionsRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateCurrencyWithSuggestionViewModel
            {
                ShowSuggestions = true
            };

            // Get all currency groups for the partner
            var currencyGroups = await currencyGroupService.GetByPartner(request.PartnerId, group => group.Enabled);
            
            // Create currency groups select list
            result.CurrencyGroups = currencyGroups.Select(group => new SelectListItem(group.Name, group.Id)).ToList();

            // Get suggestions based on existing currency groups
            var suggestions = new List<CurrencyGroupSuggestionModel>();
            
            foreach (var group in currencyGroups)
            {
                var groupGames = await currencyGroupGameApi.GetByGroup(group.Id);
                
                if (groupGames != null && groupGames.Any())
                {
                    var suggestion = new CurrencyGroupSuggestionModel
                    {
                        GroupId = group.Id,
                        GroupName = group.Name,
                        PartnerId = group.PartnerId,
                        Games = groupGames.Select(game => new GameBetLimitModel
                        {
                            GameId = game.GameId,
                            GameName = game.GameName,
                            MinBet = game.MinBet,
                            MaxBet = game.MaxBet,
                            BetLevels = game.BetLevels,
                            CoinValues = game.CoinValues
                        }).ToList()
                    };
                    
                    suggestions.Add(suggestion);
                }
            }
            
            result.Suggestions = suggestions;
            
            return result;
        }
    }
} 