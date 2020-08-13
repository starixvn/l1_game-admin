using GamesAdmin.Api.GameUsers.Results;
using MediatR;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class AddSpinRequest : IRequest<AddMemberSpinResult>
    {
        public AddSpinRequest(string key, string obCustId, byte numberOfSpins) 
        {
            Key = key;
            ObCustId = obCustId;
            NumberOfSpins = numberOfSpins;
        }

        public string Key { get; }

        public string ObCustId { get; }

        public byte NumberOfSpins { get; }
    }
}
