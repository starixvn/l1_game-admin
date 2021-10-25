using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using GamesAdmin.Site.Features.Partners;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class QueryCurrencyGroupsHandler : Handler, IRequestHandler<QueryCurrencyGroupsRequest, CurrencyGroupManagementViewModel>
    {
        private readonly IPartnerService partnerService;

        public QueryCurrencyGroupsHandler(IMapper mapper, ICurrencyGroupApi api, IPartnerService partnerService) : base(mapper, api)
        {
            this.partnerService = partnerService;
        }

        public async Task<CurrencyGroupManagementViewModel> Handle(QueryCurrencyGroupsRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetPartnersForSelection(partner => true);

            if (request.Model.Currencies == null || !request.Model.Currencies.Any())
            {
                return new CurrencyGroupManagementViewModel
                {
                    Models = new List<CurrencyGroupModel>(),
                    PartnerItems = partners,
                };
            }

            var groups = await Api.Query(request.Model);

            return new CurrencyGroupManagementViewModel
            {
                Models = groups,
                PartnerItems = partners,
            };
        }
    }
}