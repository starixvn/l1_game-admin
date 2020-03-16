using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class ToogleOnOffRequest : IRequest<bool>
    {
        public ToogleOnOffRequest(string id) 
        {
            Id = id;
        }

        public string Id { get; }
    }
}
