using MediatR;
using System;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UnderMaintenanceRequest : IRequest<bool>
    {
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public bool IsUM { get; set; }
    }
}
