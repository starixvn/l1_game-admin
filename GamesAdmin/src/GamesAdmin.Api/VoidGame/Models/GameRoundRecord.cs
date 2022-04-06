using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.VoidGame.Models
{
    public interface IGameRound
    {
        bool Exist();
    }

    public class GameRoundRecord<T1> : IGameRound
        where T1 : BaseRoundEntity
    {
        public GameRoundRecord(IList<T1> records, bool isFromArchive)
        {
            Records = records;
            IsFromArchive = isFromArchive;
        }

        public IList<T1> Records { get; }

        public bool IsFromArchive { get; }

        public bool Exist() => Records.Count > 0;
    }
}