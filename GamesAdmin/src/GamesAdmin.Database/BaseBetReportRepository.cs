using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public abstract class BaseBetReportRepository
    {
        protected readonly IMongoDatabase database;

        public BaseBetReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        protected async Task<(TRoundEntity, IEnumerable<BetReportEntity>)> GetAsync<TRoundEntity, TBetEntity>(long roundId, string nickname, bool excludeBot) where TRoundEntity : BaseRoundEntity where TBetEntity : BaseBetEntity
        {
            var betCollection = database.GetCollection<TBetEntity>(Document.GetCollectionName(typeof(TBetEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

            var aggregateBuilder = betCollection.Aggregate().Match(x => x.Round == roundId)
                 .Lookup<TBetEntity, AccountInfoEntity, BetEntityWithAccountInfos>(
                        foreignCollection: accountInfoCollection,
                        localField: x => x.Uid,
                        foreignField: x => x.UserId,
                        @as: r => r.AccountInfos)
                  .Unwind<BetEntityWithAccountInfos, BetEntityWithAccountInfoUnwind>(x => x.AccountInfos)
                  .Match(x => (string.IsNullOrWhiteSpace(nickname) || x.AccountInfos.Name == nickname) && (!excludeBot || !x.AccountInfos.IsBot) && x.AccountInfos.Currency != "UUS")
                  .AppendStage<BetEntityWithAccountInfoUnwind>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                  .Lookup<BetEntityWithAccountInfoUnwind, AccountEntity, BetEntityWithAccountInfoUnwind>(
                        foreignCollection: accountCollection,
                        localField: x => x.UserId,
                        foreignField: x => x.Id,
                        @as: r => r.Accounts
                  )
                  .Unwind<BetEntityWithAccountInfoUnwind, BetBigSmallEntityReportUnwind>(x => x.Accounts)
                  .Project(x => new BetReportEntity
                  {
                      Time = x.Time,
                      Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                      Nickname = x.AccountInfos.Name,
                      SelectBig = x.Select,
                      Bet = x.Bet,
                      BetWin = x.Betwin,
                      Back = x.Back
                  });

            var records = aggregateBuilder.ToList();

            var roundResult = await database.GetCollection<TRoundEntity>(Document.GetCollectionName(typeof(TRoundEntity))).Find(x => x.Number == roundId).FirstOrDefaultAsync();

            return (roundResult, records);
        }
    }

    class BetEntityWithAccountInfoUnwind : BaseBetEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public IEnumerable<AccountEntity> Accounts { get; set; }
    }

    class BetEntityWithAccountInfos : BaseBetEntity
    {
        public IEnumerable<AccountInfoEntity> AccountInfos { get; set; }
    }

    class BetBigSmallEntityReportUnwind : BaseBetEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public AccountEntity Accounts { get; set; }
    }

    public class BetReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public bool SelectBig { get; set; }

        public decimal Bet { get; set; }

        public decimal BetWin { get; set; }

        public decimal Back { get; set; }
    }
}
