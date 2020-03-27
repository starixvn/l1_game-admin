using MediatR;
using System;

namespace GamesAdmin.Api.UM.Requests
{
    public class ScheduleRequest : IRequest<bool>
    {
        public ScheduleRequest(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }
    }
}
