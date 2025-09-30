using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.VoidGame.Models
{
    public interface IGameTicket<T>
    {
        bool Exist();

        T GetBet();
    }

    public class GameTicketRecord<T1, T2> : IGameTicket<T1>
        where T1 : class
        where T2 : class
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

        public T1 GetBet() => OneRecord;
    }

    public class GameTicketRecord<T1> : IGameTicket<T1>
        where T1 : class
    {
        public GameTicketRecord(IList<T1> betRecords, bool isFromArchive)
        {
            BetRecords = betRecords;
            IsFromArchive = isFromArchive;
        }

        public IList<T1> BetRecords { get; set; }

        public bool IsFromArchive { get; set; }

        public bool Exist() => BetRecords.Count > 0;

        public T1 GetBet() => BetRecords.FirstOrDefault();
    }

    public class BlackjackRecord : IGameTicket<BlackjackGameBetEntity>
    {
        public BlackjackRecord(BlackjackGameBetEntity betRecord, bool isFromArchive, List<long> handIds)
        {
            BetRecord = betRecord;
            IsFromArchive = isFromArchive;
            HandIds = handIds;
        }

        public BlackjackGameBetEntity BetRecord { get; }

        public long RoundId => BetRecord !=null ? BetRecord.Round : 0;

        public bool IsFromArchive { get; }

        public List<long> HandIds { get; set; }

        public bool IsRunningTicket => BetRecord != null && !BetRecord.Paid;

        public bool Exist() => BetRecord != null;

        public BlackjackGameBetEntity GetBet() => BetRecord;
    }

    public class RoundRecord<T1> : IGameTicket<T1>
        where T1 : class
    {
        public RoundRecord(IList<T1> roundRecords, bool isFromArchive)
        {
            RoundRecords = roundRecords;
            IsFromArchive = isFromArchive;
        }

        public IList<T1> RoundRecords { get; set; }

        public bool IsFromArchive { get; set; }

        public bool Exist() => RoundRecords.Count > 0;

        public T1 GetBet() => RoundRecords.FirstOrDefault();
    }
}