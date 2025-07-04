using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetCurrencyGroupGameSuggestionsHandler : Handler, IRequestHandler<GetCurrencyGroupGameSuggestionsRequest, CreateCurrencyWithSuggestionViewModel>
    {
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly IGroupCurrencyGameApi currencyGroupGameApi;

        public GetCurrencyGroupGameSuggestionsHandler(
            IMapper mapper, 
            ICurrencyApi api, 
            ICurrencyGroupService currencyGroupService,
            IGroupCurrencyGameApi currencyGroupGameApi) : base(mapper, api)
        {
            this.currencyGroupService = currencyGroupService;
            this.currencyGroupGameApi = currencyGroupGameApi;
        }

        public async Task<CreateCurrencyWithSuggestionViewModel> Handle(GetCurrencyGroupGameSuggestionsRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateCurrencyWithSuggestionViewModel
            {
                ShowSuggestions = true,
                RateToBase = request.Rate
            };

            // Get the specific currency group
            var currencyGroups = await currencyGroupService.GetByPartner(null, group => group.Id == request.CurrencyGroupId);
            var selectedGroup = currencyGroups.FirstOrDefault();
            
            if (selectedGroup != null)
            {
                // Get games for this currency group
                var groupGames = await currencyGroupGameApi.GetByGroup(request.CurrencyGroupId);
                
                if (groupGames != null && groupGames.Any())
                {
                    var suggestion = new CurrencyGroupSuggestionModel
                    {
                        GroupId = selectedGroup.Id,
                        GroupName = selectedGroup.Name,
                        PartnerId = selectedGroup.PartnerId,
                        Games = groupGames.Select(game => new GameBetLimitModel
                        {
                            GameId = game.GameId,
                            GameName = game.GameName,
                            MinBet = game.MinBet / request.Rate, // Recalculate based on rate
                            MaxBet = game.MaxBet / request.Rate, // Recalculate based on rate
                            BetLevels = game.BetLevels?.Select(level => level / request.Rate).ToArray(), // Recalculate bet levels
                            CoinValues = game.CoinValues?.Select(coin => coin / request.Rate).ToArray() // Recalculate coin values
                        }).ToList()
                    };
                    
                    result.Suggestions = new List<CurrencyGroupSuggestionModel> { suggestion };
                }
            }
            
            return result;
        }
    }
} 