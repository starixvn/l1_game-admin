using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IJackpotRepository
    {
        Task<IEnumerable<JackpotReportEntity>> GetAsync(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot, string operatorId, IEnumerable<string> siteNames);
        Task<IEnumerable<JackpotPool>> GetRunningPoolsAsync(int gameId, string operatorId, IEnumerable<string> siteNames);
    }

    public class JackpotRepository : IJackpotRepository
    {
        private readonly IMongoDatabase database;
        private readonly IGenericRepository<GameConfigEntity> gameConfigRepository;

        public JackpotRepository(IMongoDatabase database, IGenericRepository<GameConfigEntity> gameConfigRepository)
        {
            this.database = database;
            this.gameConfigRepository = gameConfigRepository;
        }


        public async Task<IEnumerable<JackpotReportEntity>> GetAsync(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot, string operatorId, IEnumerable<string> siteNames)
        {
            var gameConfig = await gameConfigRepository.FindOneAsync(x => x.GameId == gameId);
            var jackpotCollection = database.GetCollection<JackpotEntity>($"{gameConfig.Name}_jackpots");
            var accountCollection = database.GetCollection<AccountEntity>(Document.GetCollectionName(typeof(AccountEntity)));
            var accountInfoCollection = database.GetCollection<AccountInfoEntity>(Document.GetCollectionName(typeof(AccountInfoEntity)));
            var operatorCollection = database.GetCollection<PartnerEntity>(Document.GetCollectionName(typeof(PartnerEntity)));

            var jackpots = await jackpotCollection
                .Find(jackPot => jackPot.HitTime >= fromDate && jackPot.HitTime <= toDate && jackPot.HitAt.HasValue 
                    && (string.IsNullOrWhiteSpace(operatorId) || jackPot.OperatorId == operatorId)
                    && (!siteNames.Any() || siteNames.Any(s => jackPot.SiteName.Contains(s))))
                .ToListAsync();
            var winnerNames = jackpots.Select(x => x.WinnerName).Distinct();
            var operators = await operatorCollection.Find(x => jackpots.Select(jp => jp.OperatorId).Contains(x.PartnerId)).ToListAsync();


            var acountInfos = await accountInfoCollection.Find(x => winnerNames.Contains(x.Name)).ToListAsync();
            var userIds = acountInfos.Select(x => x.UserId).Distinct().Select(x => ObjectId.Parse(x));
            var accounts = await accountCollection.Find(x => userIds.Contains(x.Id)).ToListAsync();

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
                        Rate = jackpot.Rate,
                        SiteName = jackpot.SiteName,
                        OperatorName = operators.FirstOrDefault(x => x.PartnerId == jackpot.OperatorId).Name,
                    });
                }
            }

            return result;
        }

        public async Task<IEnumerable<JackpotPool>> GetRunningPoolsAsync(int gameId, string operatorId, IEnumerable<string> siteNames)
        {
            var operatorCollection = database.GetCollection<PartnerEntity>(Document.GetCollectionName(typeof(PartnerEntity)));
            var operators = await operatorCollection.Find(x => string.IsNullOrWhiteSpace(operatorId) || x.PartnerId == operatorId).ToListAsync();
            var operatorIds = operators.Select(x => x.PartnerId);

            var jackpotConfigs = await GetJackpotConfigsAsync(operatorId, siteNames);
            var jackpotConfigIds = jackpotConfigs.Select(x => x.Id.ToString());

            var gameConfig = await gameConfigRepository.FindOneAsync(x => x.GameId == gameId);
            var jackpotCollection = database.GetCollection<JackpotEntity>($"{gameConfig.Name}_jackpots");
            var pools = await jackpotCollection.Find(x => !x.HitAt.HasValue && jackpotConfigIds.Contains(x.JackpotConfigId)).ToListAsync();

            return pools.Select(x =>
            {
                var jackpotConfig = jackpotConfigs.FirstOrDefault(jc => jc.Id == ObjectId.Parse(x.JackpotConfigId));
                return new JackpotPool
                {
                    JackpotConfigId = x.JackpotConfigId,
                    OperatorId = jackpotConfig.OperatorId,
                    OperatorName = operators.Find(o => o.PartnerId == jackpotConfig.OperatorId)?.Name,
                    SiteNames = jackpotConfig.SiteNames,
                    Prize = x.Prize,
                    Amount = x.Amount
                };
            }).OrderBy(x => x.OperatorName).ThenBy(x => x.SiteNames.Length); ;
        }

        private async Task<IEnumerable<JackpotConfigEntity>> GetJackpotConfigsAsync(string operatorId, IEnumerable<string> siteNames)
        {
            var jackpotConfigCollection = database.GetCollection<JackpotConfigEntity>(Document.GetCollectionName(typeof(JackpotConfigEntity)));
            return await jackpotConfigCollection.Find(
                x => (string.IsNullOrWhiteSpace(operatorId) || x.OperatorId == operatorId)
                    && (!siteNames.Any() || x.SiteNames.Any(siteName => siteNames.Any(siteName2 => siteName.Contains(siteName2))))
                ).ToListAsync();
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
        public string OperatorName { get; set; }
        public string SiteName { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal Rate { get; set; }
    }
}