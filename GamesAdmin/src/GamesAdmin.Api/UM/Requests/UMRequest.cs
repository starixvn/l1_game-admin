using System;
using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class UMRequest : IRequest<bool>
    {
        public UMRequest(bool IsUM, DateTimeOffset StartTime, DateTimeOffset EndTime)
        {
            this.IsUM = IsUM;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        public bool IsUM { get; }
       
        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }
    }
}
