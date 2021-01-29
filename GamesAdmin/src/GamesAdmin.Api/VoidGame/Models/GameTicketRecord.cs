using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.VoidGame.Models
{
    public class GameTicketRecord<T1, T2>
        where T1 : BaseBetEntity
        where T2 : BaseBetEntity
    {
        public GameTicketRecord(T1 oneRecord, T2 betRecord)
        {
            OneRecord = oneRecord;
            BetRecord = betRecord;
        }

        public T1 OneRecord { get; }

        public T2 BetRecord { get; }
    }
}