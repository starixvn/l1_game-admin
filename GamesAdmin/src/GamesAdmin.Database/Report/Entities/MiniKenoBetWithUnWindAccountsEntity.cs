using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Keno;
using MongoDB.Bson;

namespace GamesAdmin.Database.Report.Entities
{
    public class MiniKenoBetWithUnWindAccountsEntity : KenoMiniOnesEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public AccountEntity Accounts { get; set; }
    }
}