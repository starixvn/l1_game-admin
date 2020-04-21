using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateDisabledMessageRequest: IRequest<bool>
    {
        public UpdateDisabledMessageRequest(string name, string disabledMessage) 
        {
            Name = name;
            DisabledMessage = disabledMessage;
        }

        public string Name { get; }

        public string DisabledMessage { get; }
    }
}
