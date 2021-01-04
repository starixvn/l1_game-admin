using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IJackpotRepository
    {
        Task<IEnumerable<JackpotReportEntity>> GetAsync(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot);
    }

    public class JackpotRepository : IJackpotRepository
    {
        private readonly IMongoDatabase database;

        public JackpotRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<JackpotReportEntity>> GetAsync(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot)
        {
            var jackpotCollection = database.GetCollection<JackpotEntity>(Document.GetCollectionName(typeof(JackpotEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

            var aggregateBuilder = jackpotCollection.Aggregate().Match(x => x.HitTime >= fromDate && x.HitTime <= toDate && x.GameId == gameId && x.HitAt.HasValue)
                .SortByDescending(x => x.Id)
                .Lookup<JackpotEntity, AccountInfoEntity, JackpotEntityWithAccountInfos>(
                       foreignCollection: accountInfoCollection,
                       localField: x => x.WinnerName,
                       foreignField: x => x.Name,
                       @as: r => r.AccountInfos)
             .Unwind<JackpotEntityWithAccountInfos, JackpotEntityWithAccountInfoUnwind>(x => x.AccountInfos)
             .Match(x => (!excludeUUS || x.AccountInfos.Currency != "UUS") && (!excludeBot || !x.AccountInfos.IsBot))
             .AppendStage<JackpotEntityWithAccountInfoUnwind>("{$addFields: {userId: {$toObjectId: '$accountInfos.id'}}}")
             .Lookup<JackpotEntityWithAccountInfoUnwind, AccountEntity, JackpotEntityWithAccountInfoUnwind>(
                   foreignCollection: accountCollection,
                   localField: x => x.UserId,
                   foreignField: x => x.Id,
                   @as: r => r.Accounts
             )
            .Unwind<JackpotEntityWithAccountInfoUnwind, JackpotBetWithUnWindAccountsEntity>(x => x.Accounts);

            return await aggregateBuilder.Project(x => new JackpotReportEntity
            {
                Time = x.HitTime.Value,
                Username = x.AccountInfos.IsBot ? x.Accounts.Local.Username : x.Accounts.Local.MemberName,
                Nickname = x.WinnerName,
                RoundId = x.HitAt.Value,
                Pool = x.Pool,
                WinningAmount = x.Amount * x.WinningRatio.Value
            }).ToListAsync();
        }
    }

    public class JackpotReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal Pool { get; set; }

        public long RoundId { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal WinningAmount { get; set; }
    }

    internal class JackpotEntityWithAccountInfos : JackpotEntity
    {
        public IEnumerable<AccountInfoEntity> AccountInfos { get; set; }
    }

    internal class JackpotEntityWithAccountInfoUnwind : JackpotEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public IEnumerable<AccountEntity> Accounts { get; set; }
    }

    internal class JackpotBetWithUnWindAccountsEntity : JackpotEntity
    {
        public ObjectId UserId { get; set; }

        public AccountInfoEntity AccountInfos { get; set; }

        public AccountEntity Accounts { get; set; }
    }
}