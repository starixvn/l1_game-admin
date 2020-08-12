using GamesAdmin.Api.GameUsers.Results;
using MediatR;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class AddSpinRequest : IRequest<AddSpinResult>
    {
        public AddSpinRequest(string key, string obCustId, byte spins) 
        {
            Key = key;
            ObCustId = obCustId;
            Spins = spins;
        }

        public string Key { get; }

        public string ObCustId { get; }

        public byte Spins { get; }
    }
}
