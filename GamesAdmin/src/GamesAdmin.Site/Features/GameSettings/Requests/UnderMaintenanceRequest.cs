using MediatR;
using System;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UnderMaintenanceRequest : IRequest<bool>
    {
        public DateTimeOffset From { get; set; }

        public DateTimeOffset To { get; set; }

        public bool IsUM { get; set; }
    }
}
