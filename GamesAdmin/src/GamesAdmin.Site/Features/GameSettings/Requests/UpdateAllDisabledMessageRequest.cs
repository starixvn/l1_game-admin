using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateAllDisabledMessageRequest: IRequest<bool>
    {
        public UpdateAllDisabledMessageRequest(string disabledMessage) 
        {
            DisabledMessage = disabledMessage;
        }

        public string DisabledMessage { get; }
    }
}
