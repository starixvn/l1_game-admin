using System;
using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class UpdatePartnerUMRequest : IRequest<bool>
    {
        public UpdatePartnerUMRequest(bool IsUM, DateTimeOffset? StartTime, DateTimeOffset? EndTime, string[] Partners)
        {
            this.IsUM = IsUM;
            this.StartTime = StartTime ?? default;
            this.EndTime = EndTime ?? default;
            this.Partners = Partners;
        }

        public bool IsUM { get; }
       
        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }

        public string[] Partners { get; }
    }
}
