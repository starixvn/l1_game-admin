using GamesAdmin.Database.Entities;
using MongoDB.Bson;

namespace GamesAdmin.Database.Report.Entities
{
    public class CasinoHoldemPokerBetWithUnWindAccountsEntity : CasinoHoldemPokerBetEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public AccountEntity Accounts { get; set; }

    }
}