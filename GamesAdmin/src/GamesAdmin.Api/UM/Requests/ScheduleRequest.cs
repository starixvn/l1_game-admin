using MediatR;
using System;

namespace GamesAdmin.Api.UM.Requests
{
    public class ScheduleRequest : IRequest<bool>
    {
        public ScheduleRequest(DateTimeOffset start, DateTimeOffset end)
        {
            Start = start;
            End = end;
        }

        public DateTimeOffset Start { get; }

        public DateTimeOffset End { get; }
    }
}
