using GamesAdmin.Core.Models.Partners;
using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class GetConfigRequest : IRequest<PartnerConfig>
    {
        public GetConfigRequest(string partnerId, string type)
        {
            PartnerId = partnerId;
            Type = type;
        }

        public string PartnerId { get; }
        public string Type { get; }
    }
}