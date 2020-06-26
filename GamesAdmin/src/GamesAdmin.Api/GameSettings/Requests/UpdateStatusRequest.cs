using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateStatusRequest : IRequest<bool>
    {
        public UpdateStatusRequest(string[] names, bool enabled) 
        {
            Names = names;
            Enabled = enabled;
        }

        public string[] Names { get; }

        public bool Enabled { get; }
    }
}
