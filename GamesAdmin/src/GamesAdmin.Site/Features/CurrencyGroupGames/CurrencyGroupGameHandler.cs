using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
            var currencyGroupGame = await currencyGroupGameService.Get(request.CurrencyGroupId, request.GameId);
            var result = mapper.Map<EditViewModel>(currencyGroupGame);

            return result;
        }

        public Task<ResultBase> Handle(EditRequest request, CancellationToken cancellationToken)
            => currencyGroupGameService.Update(request.CurrencyGroupGame);

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        {
            var result = new IndexViewModel();
            var chipConfigs = await chipService.GetAll();
            var currencyGroups = (await currencyGroupService.GetByPartner(request.PartnerId)).ToList();

            result.PartnerId = request.PartnerId;
            result.PartnerName = request.PartnerName;
            result.GroupSelectList = currencyGroups.Select(group => new SelectListItem(group.Name, group.Id)).ToList();
            result.ChipConfigs = chipConfigs;

            return result;
        }
    }
}