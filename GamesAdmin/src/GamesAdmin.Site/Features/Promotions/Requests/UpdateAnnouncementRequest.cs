using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class UpdateAnnouncementRequest : IRequest<bool>
    {
        public UpdateAnnouncementRequest(CreateAnnouncementItemViewModel model)
        {
            Model = model;
        }

        public CreateAnnouncementItemViewModel Model { get; }
    }
}
