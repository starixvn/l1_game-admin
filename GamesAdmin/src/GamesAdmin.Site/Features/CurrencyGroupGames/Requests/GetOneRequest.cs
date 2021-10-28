using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class GetOneRequest : IRequest<EditViewModel>
    {
        public GetOneRequest(string groupId, string gameName)
        {
            GroupId = groupId;
            GameName = gameName;
        }

        public string GroupId { get;  }

    
        public string GameName { get; }
    }
}
