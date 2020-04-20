using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateStatusRequest: IRequest<bool>
    {
        public UpdateStatusRequest(string name, bool enabled) 
        {
            Name = name;
            Enabled = enabled;
        }

        public string Name { get; }
        public bool Enabled { get; }
    }
}
