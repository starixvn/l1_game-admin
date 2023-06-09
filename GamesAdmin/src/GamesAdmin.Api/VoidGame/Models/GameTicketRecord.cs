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

        public IList<T1> BetRecords { get; set; }

        public bool IsFromArchive { get; set; }

        public bool Exist() => BetRecords.Count > 0;
    }

    public class BlackjackRecord : IGameTicket
    {
        public BlackjackRecord(BlackjackBetEntity betRecord, bool isFromArchive, List<long> handIds)
        {
            BetRecord = betRecord;
            IsFromArchive = isFromArchive;
            HandIds = handIds;
        }

        public BlackjackBetEntity BetRecord { get; }

        public long RoundId => BetRecord !=null ? BetRecord.Round : 0;

        public bool IsFromArchive { get; }

        public List<long> HandIds { get; set; }

        public bool IsRunningTicket => BetRecord != null && !BetRecord.Paid;

        public bool Exist() => BetRecord != null;
    }
}