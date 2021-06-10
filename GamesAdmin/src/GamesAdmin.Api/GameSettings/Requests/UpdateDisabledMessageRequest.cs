using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateDisabledMessageRequest: IRequest<bool>
    {
        public UpdateDisabledMessageRequest(Dictionary<string, string> messages, string name = "", bool isApplyAll = false) 
        {
            Name = name;
            Messages = messages;
            IsApplyAll = isApplyAll;
        }

        public string Name { get; }

        public Dictionary<string, string> Messages { get; }

        public bool IsApplyAll { get; }
    }
}
