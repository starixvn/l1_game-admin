using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Baccarat;
using MongoDB.Bson;

namespace GamesAdmin.Database.Report.Entities
{
    public class BaccaratBetWithUnWindAccountsEntity : BaccaratBetEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public AccountEntity Accounts { get; set; }
    }
}