using GamesAdmin.Core.Models.Partners;
using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class GetByIdRequest : IRequest<Partner>
    {
        public GetByIdRequest(string partnerId)
        {
            PartnerId = partnerId;
        }

        public string PartnerId { get; }
    }
}
