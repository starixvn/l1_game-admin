using GamesAdmin.Core.Models.Partner;
using MediatR;

namespace GamesAdmin.Api.Partner.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(PartnerInfo partner)
        {
            Partner = partner;
        }

        public PartnerInfo Partner { get; }
    }
}
