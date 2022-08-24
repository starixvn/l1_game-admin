using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetAnnouncementByIdRequest : IRequest<Notification>
    {
        public GetAnnouncementByIdRequest(string id)
        {
            Id = id;
        }
        public string Id { get; }
    }
}
