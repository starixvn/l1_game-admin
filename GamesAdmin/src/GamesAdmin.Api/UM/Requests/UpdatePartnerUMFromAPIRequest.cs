using System;
using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class UpdatePartnerUMFromAPIRequest : IRequest<bool>
    {
        public UpdatePartnerUMFromAPIRequest(string PartnerId, bool IsUM, DateTimeOffset? StartTime, DateTimeOffset? EndTime)
        {
            this.IsUM = IsUM;
            this.StartTime = StartTime ?? default;
            this.EndTime = EndTime ?? default;
            this.PartnerId = PartnerId;
        }

        public bool IsUM { get; }

        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }

        public string PartnerId { get; }
    }
}
