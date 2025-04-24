using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.GeneralGameSettings.Requests
{
    public class GetGameByPartnerAndTypeRequest : IRequest<IEnumerable<GeneralGameSetting>>
    {
        public GetGameByPartnerAndTypeRequest(string partnerId, int gameType)
        {
            PartnerId = partnerId;
            GameType = gameType;
        }

        public string PartnerId { get; }
        public int GameType { get; }
    }
}
