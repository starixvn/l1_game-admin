using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.VoidGame.Requests;
using GamesAdmin.Site.Features.VoidGame.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.VoidGame
{
    public class VoidGameHandler : IRequestHandler<VoidTicketRequest, VoidGameResult>
        ,IRequestHandler<IndexViewRequest, IndexViewModel>
    {
        private readonly IVoidGameService voidGameService;
        private readonly IPartnerService partnerService;

        public VoidGameHandler(IVoidGameService voidGameService, IPartnerService partnerService)
        {
            this.voidGameService = voidGameService;
            this.partnerService = partnerService;
        }

        public Task<VoidGameResult> Handle(VoidTicketRequest request, CancellationToken cancellationToken)
        => voidGameService.VoidTicket(request);

        public async Task<IndexViewModel> Handle(IndexViewRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetAll();
            var viewModel = new IndexViewModel();

            viewModel.Partners = partners.Select(partner => new SelectListItem { Text= partner.Name, Value = partner.PartnerId}).ToList();

            return viewModel;
        }
    }
}