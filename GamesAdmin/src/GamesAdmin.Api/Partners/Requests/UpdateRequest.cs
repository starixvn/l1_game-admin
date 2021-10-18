using GamesAdmin.Core.Models.Partners;
using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(Partner partner)
        {
            Partner = partner;
        }

        public Partner Partner { get; }
    }
}
