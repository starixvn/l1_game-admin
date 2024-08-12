using System.Collections.Generic;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetAnnouncementByStatusRequest : IRequest<IEnumerable<AnnouncementViewModel>>
    {
        public GetAnnouncementByStatusRequest(string status, string type)
        {
            Status = status;
            Type = type;

        }
        public string Status { get; }

        public string Type { get; }
    }
}
