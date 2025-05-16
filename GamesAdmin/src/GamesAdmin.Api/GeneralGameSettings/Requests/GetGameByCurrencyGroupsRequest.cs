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

        public List<int> GameIds { get; }

        public GetGameByCurrencyGroupsRequest(int gameType, string currencyGroups, string gameIds)
        {
            GameType = gameType;
            CurrencyGroupIds = currencyGroups?.Split(',').Select(x => x.Trim()).ToList() ?? new List<string>();
            GameIds = string.IsNullOrWhiteSpace(gameIds)
                    ? new List<int>()
                    : gameIds
                        .Split(',')
                        .Select(x => x.Trim())
                        .Where(x => int.TryParse(x, out _))
                        .Select(int.Parse)
                        .ToList();
                        }
    }
}
