using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IJackpotRepository
    {
        Task<IEnumerable<JackpotReportEntity>> GetAsync(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot);
    }

    public class JackpotRepository : IJackpotRepository
    {
        private readonly IMongoDatabase database;

        public JackpotRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<JackpotReportEntity>> GetAsync(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot)
        {
            var jackpotCollection = database.GetCollection<JackpotEntity>(Document.GetCollectionName(typeof(JackpotEntity)));
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));

            var jackpots = await jackpotCollection
                .Find(jackPot => jackPot.HitTime >= fromDate && jackPot.HitTime <= toDate && jackPot.GameId == gameId && jackPot.HitAt.HasValue)
                .ToListAsync();
            var accounts = accountCollection.AsQueryable().ToList();
            var acountInfos = accountInfoCollection.AsQueryable().ToList();

            var result = new List<JackpotReportEntity>();

            foreach (var jackpot in jackpots)
            {
                var accountInfo = acountInfos.Find(x => x.Name == jackpot.WinnerName);
                if ((!excludeUUS || accountInfo.Currency != "UUS") && (!excludeBot || !accountInfo.IsBot))
                {
                    var account = accounts.Find(x => x.Id == ObjectId.Parse(accountInfo.UserId));

                    result.Add(new JackpotReportEntity
                    {
                        Time = jackpot.HitTime.Value,
                        Username = account.Local.MemberName ?? account.Local.Username,
                        Nickname = jackpot.WinnerName,
                        RoundId = jackpot.HitAt.Value,
                        Pool = jackpot.Pool,
                        Prize = jackpot.Prize,
                        WinningAmount = jackpot.Amount * jackpot.WinningRatio.Value,
                        OperatorId = jackpot.OperatorId,
                        Currency = jackpot.Currency,
                        ExchangeRateId = jackpot.ExchangeRateId,
                        Rate = jackpot.Rate
                    });
                }
            }

            return result;
        }
    }

    public class JackpotReportEntity
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal Pool { get; set; }

        public string Prize { get; set; }

        public long RoundId { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal WinningAmount { get; set; }

        public string Currency { get; set; }

        public string OperatorId { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal Rate { get; set; }
    }
}