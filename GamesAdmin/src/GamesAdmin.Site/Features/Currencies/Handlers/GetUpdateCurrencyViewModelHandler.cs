using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetUpdateCurrencyViewModelHandler : Handler, IRequestHandler<GetUpdateCurrencyViewModelRequest, UpdateCurrencyViewModel>
    {
        private readonly IMediator mediator;

        public GetUpdateCurrencyViewModelHandler(IMapper mapper, ICurrencyApi api, IMediator mediator) : base(mapper, api)
        {
            this.mediator = mediator;
        }

        public async Task<UpdateCurrencyViewModel> Handle(GetUpdateCurrencyViewModelRequest request, CancellationToken cancellationToken)
        {
            var partners = await mediator.Send(new GetPartnersForSelectionRequest());
            var currency = await Api.GetById(request.Id);
            var viewModel = Mapper.Map<UpdateCurrencyViewModel>(currency);

            viewModel.Partners = partners;

            return viewModel;
        }
    }
}