using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class QueryCurrencyGroupsHandler : Handler, IRequestHandler<QueryCurrencyGroupsRequest, CurrencyGroupManagementViewModel>
    {
        private readonly IMediator mediator;

        public QueryCurrencyGroupsHandler(IMapper mapper, ICurrencyGroupApi api, IMediator mediator) : base(mapper, api)
        {
            this.mediator = mediator;
        }

        public async Task<CurrencyGroupManagementViewModel> Handle(QueryCurrencyGroupsRequest request, CancellationToken cancellationToken)
        {
            var groups = await Api.Query(request.Model);
            var partners = await mediator.Send(new GetPartnersForSelectionRequest());

            return new CurrencyGroupManagementViewModel
            {
                Models = groups,
                PartnerItems = partners,
            };
        }
    }
}