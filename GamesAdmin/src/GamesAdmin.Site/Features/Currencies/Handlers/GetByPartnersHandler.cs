using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.Partners;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetByPartnersHandler : Handler, IRequestHandler<GetByPartnersRequest, CurrencyManagementViewModel>
    {
        private readonly ICurrencyGroupApi currencyGroupApi;

        private readonly IPartnerService partnerService;

        public GetByPartnersHandler(IMapper mapper, ICurrencyApi api, ICurrencyGroupApi currencyGroupApi, IPartnerService partnerService) : base(mapper, api)
        {
            this.currencyGroupApi = currencyGroupApi;
            this.partnerService = partnerService;
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

            var partners = await partnerService.GetPartners(partner => request.Model.PartnerIds.Contains(partner.PartnerId));

            var groupIds = models
                .Select(model => model.GroupId)
                .Where(groupId => !string.IsNullOrWhiteSpace(groupId))
                .Distinct()
                .ToList();

            var currencyGroups = groupIds.Any() ? await currencyGroupApi.Query(string.Join(',', groupIds)) : new List<CurrencyGroupModel>();

            return new CurrencyManagementViewModel
            {
                Models = models,
                Partners = partners,
                CurrencyGroups = currencyGroups
            };
        }
    }
}