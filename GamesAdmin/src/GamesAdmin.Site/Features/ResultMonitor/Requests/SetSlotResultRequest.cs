using MediatR;

namespace GamesAdmin.Site.Features.ResultMonitor.Requests
{
    public class SetSlotResultRequest : IRequest<bool>
    {
        public string GameConfigName { get; set; }

        public string Nickname { get; set; }

        public byte[] Results { get; set; }
    }
}