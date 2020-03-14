using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateEnableRequest : IRequest<bool>
    {
        public UpdateEnableRequest(string id, bool enabled) 
        {
            Id = id;
            Enabled = enabled;
        }

        public string Id { get; }

        public bool Enabled { get; }
    }
}
