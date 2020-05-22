using GamesAdmin.Site.Features.Notifications.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications.Requests
{
    public class GetByIdRequest: IRequest<EditViewModel>
    {
        public GetByIdRequest(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
