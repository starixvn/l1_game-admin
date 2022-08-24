using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class UpdateAnnouncementRequest : IRequest<bool>
    {
        public UpdateAnnouncementRequest(Notification model)
        {
            Model = model;
        }

        public Notification Model { get; }
    }
}
