using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetByPartnerHandler : Handler, IRequestHandler<GetByPartnerRequest, CurrencyManagementViewModel>
    {
        private readonly IMediator mediator;

        public GetByPartnerHandler(IMapper mapper, ICurrencyApi api, IMediator mediator) : base(mapper, api)
        {
            this.mediator = mediator;
        }

        public async Task<CurrencyManagementViewModel> Handle(GetByPartnerRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<CurrencyModel> models;

            try
            {
                models = await Api.GetByPartner(request.Model);
            }
            catch (Exception)
            {
                models = new List<CurrencyModel>();
            }

            var partners = await mediator.Send(new GetAllPartnerRequest());

            return new CurrencyManagementViewModel
            {
                Models = models,
                Partners = partners
            };
        }
    }
}