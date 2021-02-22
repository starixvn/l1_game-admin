using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateDisabledMessageRequest: IRequest<bool>
    {
        public UpdateDisabledMessageRequest(string name, string message, bool isApplyAll = false) 
        {
            Name = name;
            Message = message;
            IsApplyAll = isApplyAll;
        }

        public string Name { get; }

        public string Message { get; }

        public bool IsApplyAll { get; }
    }
}
