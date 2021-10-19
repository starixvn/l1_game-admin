using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class IndexHandler : Handler, IRequestHandler<IndexRequest, IndexViewModel>
    {
        private readonly IMediator mediator;

        public IndexHandler(IMapper mapper, ICurrencyApi api, IMediator mediator) : base(mapper, api)
        {
            this.mediator = mediator;
        }

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        {
            var partners = await mediator.Send(new GetPartnersForSelectionRequest());
            var allPartnerItem = new SelectListItem("All", string.Empty);
            var allSelections = partners.Prepend(allPartnerItem);

            return new IndexViewModel
            {
                Partners = allSelections
            };
        }
    }
}