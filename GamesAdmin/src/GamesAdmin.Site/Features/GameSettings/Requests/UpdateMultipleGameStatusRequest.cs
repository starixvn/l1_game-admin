using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateMultipleGameStatusRequest: IRequest<bool>
    {
        public UpdateMultipleGameStatusRequest(string[] names, bool enabled)
        {
            Names = names;
            Enabled = enabled;
        }

        public string[] Names { get; }

        public bool Enabled { get; }
    }
}
