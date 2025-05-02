using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Api.GeneralGameSettings.Requests
{
    public class GetGameByCurrencyGroupsRequest : IRequest<IEnumerable<GeneralGameSetting>>
    {
        public int GameType { get; }
        public List<string> CurrencyGroupIds { get; }

        public GetGameByCurrencyGroupsRequest(int gameType, string currencyGroups)
        {
            GameType = gameType;
            CurrencyGroupIds = currencyGroups?.Split(',').Select(x => x.Trim()).ToList() ?? new List<string>();
        }
    }
}
