using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetAnnouncementByIdRequest : IRequest<CreateAnnouncementItemViewModel>
    {
        public GetAnnouncementByIdRequest(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
