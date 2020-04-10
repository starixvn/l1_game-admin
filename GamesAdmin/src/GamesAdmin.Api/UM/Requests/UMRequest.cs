using System;
using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class UMRequest : IRequest<bool>
    {
        public UMRequest(bool isUM, DateTimeOffset startTime, DateTimeOffset endTime)
        {
            IsUM = isUM;
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool IsUM { get; }

        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }
    }
}
