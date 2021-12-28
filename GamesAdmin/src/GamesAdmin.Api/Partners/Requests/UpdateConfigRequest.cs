using GamesAdmin.Core.Models.Partners;
using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class UpdateConfigRequest : IRequest<bool>
    {
        public UpdateConfigRequest(PartnerConfig config)
        {
            Config = config;
        }

        public PartnerConfig Config { get; }
    }
}