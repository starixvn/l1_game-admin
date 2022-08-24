using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetAnnouncementByStatusRequest : IRequest<IEnumerable<Notification>>
    {
        public GetAnnouncementByStatusRequest(string status)
        {
            Status = status;
        }
        public string Status { get; }
    }
}
