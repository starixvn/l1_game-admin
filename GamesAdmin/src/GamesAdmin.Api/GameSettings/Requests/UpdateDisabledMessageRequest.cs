using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateDisabledMessageRequest: IRequest<bool>
    {
        public UpdateDisabledMessageRequest(string name, string message) 
        {
            Name = name;
            Message = message;
        }

        public string Name { get; }

        public string Message { get; }
    }
}
