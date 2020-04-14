using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IBigSmallReportRepository
    {
        Task<(BigSmallRoundEntity, IEnumerable<BigSmallReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot);
    }

    public class BigSmallReportRepository : IBigSmallReportRepository
    {
        private readonly IMongoDatabase database;

        public BigSmallReportRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<(BigSmallRoundEntity, IEnumerable<BigSmallReportEntity>)> GetAsync(long roundId, string nickname, bool excludeBot)
        {
            var bigSmallBetCollection = database.GetCollection<BigSmallBetEntity>("taixiu_cuocs");
            var accountCollection = database.GetCollection<AccountEntity>("users");
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>("userinfos");

            var aggregateBuilder = bigSmallBetCollection.Aggregate().Match(x => x.Round == roundId)
                 .Lookup<BigSmallBetEntity, AccountInfoEntity, BigSmallEntityWithAccountInfos>(
                        foreignCollection: accountInfoCollection,
                        localField: x => x.Uid,
                        foreignField: x => x.userId,
                        @as: r => r.AccountInfos)
                  .Unwind<BigSmallEntityWithAccountInfos, BigSmallEntityWithAccountInfoUnwind>(x => x.AccountInfos)
                  .Match(x => (string.IsNullOrWhiteSpace(nickname) || x.AccountInfos.name == nickname) && (!excludeBot || !x.AccountInfos.isBot) && x.AccountInfos.currency != "UUS")
                  .AppendStage<BigSmallEntityWithAccountInfoUnwind>("{$addFields: {userId: {$toObjectId: '$uid'}}}")
                  .Lookup<BigSmallEntityWithAccountInfoUnwind, AccountEntity, BigSmallEntityWithAccountInfoUnwind>(
                        foreignCollection: accountCollection,
                        localField: x => x.UserId,
                        foreignField: x => x.Id,
                        @as: r => r.Accounts
                  )
                  .Unwind<BigSmallEntityWithAccountInfoUnwind, BigSmallEntityReportUnwind>(x => x.Accounts)
                  .Project(x => new BigSmallReportEntity
                  {
                      Time =  x.time,
                      Username = x.Accounts.local.username,
                      Nickname = x.AccountInfos.name,
                      SelectBig = x.Select,
                      Bet = x.Bet,
                      BetWin = x.Betwin,
                      Back = x.Back
                  });

            var records = aggregateBuilder.ToList();

            var roundResult = await database.GetCollection<BigSmallRoundEntity>("taixiu_phiens").Find(x => x.number == roundId).FirstOrDefaultAsync();

            return (roundResult, records);
        }
    }

    class BigSmallEntityWithAccountInfoUnwind : BigSmallBetEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public IEnumerable<AccountEntity> Accounts { get; set; }
    }

    class BigSmallEntityWithAccountInfos : BigSmallBetEntity
    {
        public IEnumerable<AccountInfoEntity> AccountInfos { get; set; }
    }

    class BigSmallEntityReportUnwind : BigSmallBetEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public AccountEntity Accounts { get; set; }
    }

    public class BigSmallReportEntity
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