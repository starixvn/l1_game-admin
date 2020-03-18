using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class IsExistRequest : IRequest<bool>
    {
        public IsExistRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
