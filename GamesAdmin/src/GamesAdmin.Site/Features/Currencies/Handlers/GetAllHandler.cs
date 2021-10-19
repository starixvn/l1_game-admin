using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetAllHandler : Handler, IRequestHandler<GetAllRequest, CurrencyManagementViewModel>
    {
        private readonly IMediator mediator;

        public GetAllHandler(IMapper mapper, ICurrencyApi api, IMediator mediator) : base(mapper, api)
        {
            this.mediator = mediator;
        }

        public async Task<CurrencyManagementViewModel> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            var models = await Api.GetAll();
            var partners = await mediator.Send(new GetPartnersForSelectionRequest());

            return new CurrencyManagementViewModel
            {
                Models = models,
                Partners = partners
            };
        }
    }
}