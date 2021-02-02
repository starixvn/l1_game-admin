using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.VoidGame.Models
{
    public class GameTicketRecord<T1, T2>
        where T1 : BaseBetEntity
        where T2 : BaseBetEntity
    {
        public GameTicketRecord(T1 oneRecord, IList<T2> betRecords, bool isFromArchive)
        {
            OneRecord = oneRecord;
            BetRecords = betRecords;
            IsFromArchive = isFromArchive;
        }

        public GameTicketRecord(IList<T2> betRecords, bool isFromArchive)
        {
            BetRecords = betRecords;
            IsFromArchive = isFromArchive;
        }

        public T1 OneRecord { get; }

        public IList<T2> BetRecords { get; }

        public bool IsFromArchive { get; }
    }

    public class GameTicketRecord<T1>
        where T1 : BaseBetEntity
    {
        public GameTicketRecord(IList<T1> betRecords, bool isFromArchive)
        {
            BetRecords = betRecords;
            IsFromArchive = isFromArchive;
        }

        public IList<T1> BetRecords { get; }

        public bool IsFromArchive { get; }
    }
}