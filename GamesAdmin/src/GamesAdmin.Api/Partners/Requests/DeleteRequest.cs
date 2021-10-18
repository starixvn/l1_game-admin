using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class DeleteRequest : IRequest<bool>
    {
        public DeleteRequest(string partnerId)
        {
            PartnerId = partnerId;
        }

        public string PartnerId { get; }
    }
}
