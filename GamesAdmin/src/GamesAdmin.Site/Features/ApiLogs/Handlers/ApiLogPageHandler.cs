using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.ApiLogs.Requests;
using GamesAdmin.Site.Features.ApiLogs.ViewModels;
using GamesAdmin.Site.Features.Partners;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.ApiLogs.Handlers
{
    public class ApiLogPageHandler : ApiLogHandler, IRequestHandler<ApiLogPageRequest, ApiLogViewModel>
    {
        private readonly IPartnerService partnerService;

        public ApiLogPageHandler(ILogApi api, IMapper mapper, IPartnerService partnerService) : base(api, mapper)
        {
            this.partnerService = partnerService;
        }

        public async Task<ApiLogViewModel> Handle(ApiLogPageRequest request, CancellationToken cancellationToken)
        {
            var partners = await partnerService.GetPartnersForSelection(partner => true);
            var logTypes = Enumeration.GetAll<ApiLogEnumeration>()
                                      .Select(log => new SelectListItem(log.DisplayName, $"{(byte)log.Type}"));

            return new ApiLogViewModel
            {
                Partners = partners,
                GameTypeId = 0,
                LogTypes = logTypes
            };
        }
    }
}