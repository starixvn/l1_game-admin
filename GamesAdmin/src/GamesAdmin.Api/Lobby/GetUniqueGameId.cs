using GamesAdmin.Core.Models.Lobby;
using System.Collections.Generic;

namespace GamesAdmin.Api.Lobby
{
    public class GetUniqueGameId : IEqualityComparer<Game>
    {
        public bool Equals(Game x, Game y)
        {
            return !(x == null || y == null) && x.Id == y.Id;
        }

        public int GetHashCode(Game obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}