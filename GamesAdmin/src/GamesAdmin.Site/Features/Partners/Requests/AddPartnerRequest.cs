using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class AddPartnerRequest : IRequest<ResultBase>
    {
        public AddPartnerRequest(Core.Models.Partners.Partner partner) 
        {
            PartnerInfo = partner;
        }

        public Core.Models.Partners.Partner PartnerInfo { get; }
    }
}
