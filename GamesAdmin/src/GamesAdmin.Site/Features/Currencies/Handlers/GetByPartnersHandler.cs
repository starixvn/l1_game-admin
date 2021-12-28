using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetByPartnersHandler : Handler, IRequestHandler<GetByPartnersRequest, CurrencyManagementViewModel>
    {
        private readonly IMediator mediator;

        public GetByPartnersHandler(IMapper mapper, ICurrencyApi api, IMediator mediator) : base(mapper, api)
        {
            this.mediator = mediator;
        }

        public async Task<CurrencyManagementViewModel> Handle(GetByPartnersRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<CurrencyModel> models;

            try
            {
                models = (request.Model.PartnerIds == null || !request.Model.PartnerIds.Any())
                    ? new List<CurrencyModel>()
                    : await Api.GetByPartners(request.Model);
            }
            catch (Exception)
            {
                models = new List<CurrencyModel>();
            }

            var partners = await mediator.Send(new GetPartnersForSelectionRequest());

            return new CurrencyManagementViewModel
            {
                Models = models,
                Partners = partners
            };
        }
    }
}