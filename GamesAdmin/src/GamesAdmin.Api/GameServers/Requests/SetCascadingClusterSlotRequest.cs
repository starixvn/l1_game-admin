using System.Text.Json.Nodes;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetCascadingClusterSlotRequest : IRequest<object>
    {
        public string gameConfigName { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public object result { get; set; }
    }
}
