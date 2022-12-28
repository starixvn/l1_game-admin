using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.VoidGame.Requests;
using GamesAdmin.Site.Features.VoidGame.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.VoidGame
{
    public class VoidGameHandler : IRequestHandler<VoidTicketRequest, VoidGameResultViewModel>
        , IRequestHandler<IndexViewRequest, IndexViewModel>
    {
        private readonly IVoidGameService voidGameService;
        private readonly IPartnerService partnerService;
        private readonly IGameSettingService gameSettingService;

        public VoidGameHandler(IVoidGameService voidGameService, 
            IPartnerService partnerService,
            IGameSettingService gameSettingService)
        {
            this.voidGameService = voidGameService;
            this.partnerService = partnerService;
            this.gameSettingService = gameSettingService;
        }

        public async Task<VoidGameResultViewModel> Handle(VoidTicketRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetAll();
            var partnerSelectList = partners.Select(partner => new SelectListItem { Text = partner.Name, Value = partner.PartnerId }).ToList();
            var result = await voidGameService.VoidTicket(request);

            return new VoidGameResultViewModel(result, partnerSelectList);
        }

        public async Task<IndexViewModel> Handle(IndexViewRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetAll();
            var games = await gameSettingService.GetAll();
            var gameIds = Enumeration.GetAll<Game>()
                .Where(x => (byte)x.GameId != 0)
                .Select(x => new SelectListItem { Text = x.DisplayName, Value = ((int)(x.GameId)).ToString() })
                .ToList();

            var newGameIds = games
                .Where(newGame => newGame.Info.GameId != 0 && !gameIds.Exists(x => x.Value == newGame.Info.GameId.ToString()))
                .Select(newGame => new SelectListItem { Text = newGame.Info.DisplayName, Value = newGame.Info.GameId.ToString() })
                .ToList();

            gameIds.AddRange(newGameIds);

            var viewModel = new IndexViewModel();

            viewModel.Partners = partners.Select(partner => new SelectListItem { Text = partner.Name, Value = partner.PartnerId }).ToList();
            viewModel.GameIds = gameIds;

            return viewModel;
        }
    }
}