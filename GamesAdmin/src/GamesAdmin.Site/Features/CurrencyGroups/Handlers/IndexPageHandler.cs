using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using GamesAdmin.Site.Features.Partners;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class IndexPageHandler : Handler, IRequestHandler<IndexPageRequest, IndexViewModel>
    {
        private readonly IPartnerService partnerService;

        public IndexPageHandler(IMapper mapper, ICurrencyGroupApi api, IPartnerService partnerService) : base(mapper, api)
        {
            this.partnerService = partnerService;
        }

        public async Task<IndexViewModel> Handle(IndexPageRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetAll();
            var currencyGroups = await Api.GetAll();
            var groupIds = currencyGroups.Select(group => group.Id).AsEnumerable();

            return new IndexViewModel
            {
                Partners = partners,
                CurrencyGroupModels = currencyGroups,
                CurrencyGroupItems = new MultiSelectList(currencyGroups, "Id", "Name", groupIds)
            };
        }
    }
}