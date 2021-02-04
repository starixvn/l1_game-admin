using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.VoidGame.Models
{
    public interface IGameTicket
    {
        bool Exist();
    }

    public class GameTicketRecord<T1, T2> : IGameTicket
        where T1 : BaseBetEntity
        where T2 : BaseBetEntity
    {
        public GameTicketRecord(T1 oneRecord, IList<T2> betRecords, bool isFromArchive)
        {
            OneRecord = oneRecord;
            BetRecords = betRecords;
            IsFromArchive = isFromArchive;
        }

        public T1 OneRecord { get; }

        public IList<T2> BetRecords { get; }

        public bool IsFromArchive { get; }

        public bool Exist() => OneRecord != null && BetRecords.Count > 0;
    }

    public class GameTicketRecord<T1> : IGameTicket
        where T1 : BaseBetEntity
    {
        public GameTicketRecord(IList<T1> betRecords, bool isFromArchive)
        {
            BetRecords = betRecords;
            IsFromArchive = isFromArchive;
        }

        public IList<T1> BetRecords { get; }

        public bool IsFromArchive { get; }

        public bool Exist() => BetRecords.Count > 0;
    }

    public class GameRoundTicketRecord<T1, T2> : IGameTicket
        where T1 : BaseRoundEntity
        where T2 : BaseBetEntity
    {
        public GameRoundTicketRecord(T1 roundRecord, IList<T2> betRecords, bool isFromArchive)
        {
            RoundRecord = roundRecord;
            BetRecords = betRecords;
            IsFromArchive = isFromArchive;
        }

        public T1 RoundRecord { get; }

        public IList<T2> BetRecords { get; }

        public bool IsFromArchive { get; }

        public bool Exist() => RoundRecord != null && BetRecords.Count > 0;
    }
}