using System;
using MediatR;

namespace GamesAdmin.Api.UM.Requests
{
    public class ServerApiUMRequest : IRequest<bool>
    {
        public ServerApiUMRequest(bool IsUM, bool IsPartnerUm, DateTimeOffset? StartTime, DateTimeOffset? EndTime, string[] Partners = null)
        {
            this.IsUM = IsUM;
            this.StartTime = (StartTime ?? default).ToUniversalTime();
            this.EndTime = (EndTime ?? default).ToUniversalTime();
            this.Partners = Partners;
            this.IsPartnerUm = IsPartnerUm;
        }

        public bool IsUM { get; }

        public bool IsPartnerUm { get; }
       
        public DateTimeOffset StartTime { get; }

        public DateTimeOffset EndTime { get; }

        public string[] Partners { get; }
    }
}
