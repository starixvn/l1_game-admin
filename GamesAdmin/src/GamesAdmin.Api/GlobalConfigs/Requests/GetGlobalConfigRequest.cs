using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GlobalConfigs.Requests
{
    public class GetGlobalConfigRequest : IRequest<GlobalConfig>
    {
        public GetGlobalConfigRequest(GlobalConfigType type)
        {
            Type = type;
        }

        public GlobalConfigType Type { get; }
    }
}