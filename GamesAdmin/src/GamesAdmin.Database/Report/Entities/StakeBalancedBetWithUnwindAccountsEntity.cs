using GamesAdmin.Database.Entities;
using MongoDB.Bson;

namespace GamesAdmin.Database.Report.Entities
{
    public class StakeBalancedBetWithUnwindAccountsEntity : BaseBetEntity
    {
        public decimal? Back { get; set; }

        public decimal? Bet { get; set; }

        public bool? Select { get; set; }

        public decimal Refund { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public bool Win { get; set; }

        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public AccountEntity Accounts { get; set; }
    }
}
