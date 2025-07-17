using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class GetOneRequest : IRequest<EditViewModel>
    {
        public GetOneRequest(string groupId, string gameName, bool isCopySetting)
        {
            GroupId = groupId;
            GameName = gameName;
            IsCopySetting = isCopySetting;
        }

        public string GroupId { get;  }

        public string GameName { get; }

        public bool IsCopySetting { get; }
    }
}
