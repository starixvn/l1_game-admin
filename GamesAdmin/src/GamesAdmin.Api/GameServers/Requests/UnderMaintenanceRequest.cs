using System;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class UnderMaintenanceRequest : IRequest<bool>
    {
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public bool IsUM { get; set; }
    }
}
