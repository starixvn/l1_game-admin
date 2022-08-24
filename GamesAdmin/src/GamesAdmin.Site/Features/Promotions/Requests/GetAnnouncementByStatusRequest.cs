using System.Collections.Generic;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetAnnouncementByStatusRequest : IRequest<IEnumerable<AnnouncementViewModel>>
    {
        public GetAnnouncementByStatusRequest(string status)
        {
            Status = status;
        }
        public string Status { get; }
    }
}
