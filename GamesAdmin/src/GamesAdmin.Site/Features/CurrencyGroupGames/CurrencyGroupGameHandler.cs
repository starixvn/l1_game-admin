using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Chips;
using GamesAdmin.Site.Features.CurrencyGroupGames.Requests;
using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using GamesAdmin.Site.Features.CurrencyGroups;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.CurrencyGroupGames
{
    public class CurrencyGroupGameHandler
        : IRequestHandler<GetOneRequest, EditViewModel>
        , IRequestHandler<EditRequest, ResultBase>
        , IRequestHandler<IndexRequest, IndexViewModel>
    {
        private readonly ICurrencyGroupGameService currencyGroupGameService;
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly IChipService chipService;
        private readonly IMapper mapper;

        public CurrencyGroupGameHandler(ICurrencyGroupGameService currencyGroupGameService, ICurrencyGroupService currencyGroupService, IChipService chipService,
            IMapper mapper)
        {
            this.currencyGroupGameService = currencyGroupGameService;
            this.currencyGroupService = currencyGroupService;
            this.chipService = chipService;
            this.mapper = mapper;
        }

        public async Task<EditViewModel> Handle(GetOneRequest request, CancellationToken cancellationToken)
        {
            var gameId = (int)Enumeration.FromValue<Game>(request.GameName).GameId;
            var currencyGroupGame = await currencyGroupGameService.Get(request.GroupId, gameId);
            var result = mapper.Map<EditViewModel>(currencyGroupGame) ?? new EditViewModel();
            var chipConfigs = await chipService.GetAll();

            if (currencyGroupGame?.StakeLevels == null)
            {
                result.StakeLevels = new StakeLevel[] {
                    new StakeLevel(0,0,0),
                    new StakeLevel(0,0,0),
                    new StakeLevel(0,0,0)
                };
            }

            result.GameId = gameId;
            result.GroupId = request.GroupId;
            result.AllChips = chipConfigs.OrderBy(chip => chip.Value).ToList();
            result.MaxBetChoiceSetting = GenerateMaxBetPerChoices(request.GameName, currencyGroupGame?.MaxBetPerChoices);

            return result;
        }

        public Task<ResultBase> Handle(EditRequest request, CancellationToken cancellationToken)
            => currencyGroupGameService.Update(request.CurrencyGroupGame);

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        {
            var result = new IndexViewModel();
            var currencyGroups = (await currencyGroupService.GetByPartner(request.PartnerId)).ToList();

            result.PartnerId = request.PartnerId;
            result.PartnerName = request.PartnerName;
            result.GroupSelectList = currencyGroups.Select(group => new SelectListItem(group.Name, group.Id)).ToList();

            return result;
        }

        private static List<GameSettings.ViewModels.MaxBetChoiceViewModel> GenerateMaxBetPerChoices(string gameName, Dictionary<string, double> maxBetPerChoices)
        {
            if (string.IsNullOrWhiteSpace(gameName) || !Enumeration.Contains<Game>(gameName))
            {
                return new List<GameSettings.ViewModels.MaxBetChoiceViewModel>();
            }

            var betChoices = BetChoiceMapper.Map(Enumeration.FromValue<Game>(gameName));

            return betChoices?.Select(choice => new GameSettings.ViewModels.MaxBetChoiceViewModel(
                choice.Value,
                maxBetPerChoices != null && maxBetPerChoices.ContainsKey(choice.Value)  ? maxBetPerChoices[choice.Value] : 0,
                choice.DisplayName)).ToList();
        }
    }
}