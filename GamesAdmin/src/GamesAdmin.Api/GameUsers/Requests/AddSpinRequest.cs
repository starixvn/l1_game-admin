using MediatR;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class AddSpinRequest : IRequest<bool>
    {
        public AddSpinRequest(string obCustId, byte spins) 
        {
            ObCustId = obCustId;
            Spins = spins;
        }

        public string ObCustId { get; }

        public byte Spins { get; }
    }
}
