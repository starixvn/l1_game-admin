using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateBetaVersionRequest : IRequest<bool>
    {
        public UpdateBetaVersionRequest(string[] names, bool isBeta)
        {
            Names = names;
            IsBeta = isBeta;
        }

        public string[] Names { get; }

        public bool IsBeta { get; }
    }
}
