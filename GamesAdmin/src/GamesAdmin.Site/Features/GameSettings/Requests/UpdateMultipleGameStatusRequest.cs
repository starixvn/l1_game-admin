using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateMultipleGameStatusRequest: IRequest<bool>
    {
        public UpdateMultipleGameStatusRequest(string[] names, bool enabled, bool reload = false)
        {
            Names = names;
            Enabled = enabled;
            Reload = reload;
        }

        public string[] Names { get; }

        public bool Enabled { get; }

        public bool Reload { get; }
    }
}
