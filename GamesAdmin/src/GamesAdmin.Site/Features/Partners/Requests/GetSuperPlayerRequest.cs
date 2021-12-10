using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class GetSuperPlayerRequest : IRequest<string>
    {
        public GetSuperPlayerRequest(string partnerId)
        {
            PartnerId = partnerId;
        }

        public string PartnerId { get; set; }

    }

}