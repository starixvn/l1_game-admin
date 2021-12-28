using GamesAdmin.Site.Features.Partners.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class GetByIdRequest : IRequest<PartnerEditViewModel>
    {
        public GetByIdRequest(string partnerId)
        {
            PartnerId = partnerId;
        }

        public string PartnerId { get; }
    }
}
