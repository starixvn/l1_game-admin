using MediatR;

namespace GamesAdmin.Api.Spins.Requests
{
    public class CreateRequest : IRequest
    {
        public CreateRequest(string obCustId, string siteId, byte spins) 
        {
            ObCustId = obCustId;
            SiteId = siteId;
            Spins = spins;
        }

        public string ObCustId { get; }

        public string SiteId { get; }

        public byte Spins { get; }
    }
}
