using System;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IStatisticRepository
    {
        Task<long> GetTodayTotalBets();
    }

    public class StatisticRepository : IStatisticRepository
    {
        private readonly IMongoDatabase database;

        public StatisticRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<long> GetTodayTotalBets()
        {
            var currentDate = DateTime.Now;

            var startOfDay = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, DateTimeKind.Local).ToUniversalTime();
            var endOfDay = startOfDay.AddDays(1);

            var bigSmallOnesCollection = database.GetCollection<BigSmallOnesEntity>(Document.GetCollectionName(typeof(BigSmallOnesEntity)));
            var bigSmallTurboOnesCollection = database.GetCollection<BigSmallTurboOnesEntity>(Document.GetCollectionName(typeof(BigSmallTurboOnesEntity)));
            var evenOddOnesCollection = database.GetCollection<OddEvenOnesEntity>(Document.GetCollectionName(typeof(OddEvenOnesEntity)));
            var evenOddTurboOnesCollection = database.GetCollection<OddEvenTurboOnesEntity>(Document.GetCollectionName(typeof(OddEvenTurboOnesEntity)));

            var fishPrawnCrabBetCollection = database.GetCollection<FishPrawnCrabBetEntity>(Document.GetCollectionName(typeof(FishPrawnCrabBetEntity)));
            var shakeThePlateBetCollection = database.GetCollection<ShakeThePlateBetEntity>(Document.GetCollectionName(typeof(ShakeThePlateBetEntity)));
            var dragonTigerBetCollection = database.GetCollection<DragonTigerBetEntity>(Document.GetCollectionName(typeof(DragonTigerBetEntity)));

            var bigSmallTotalBets = await bigSmallOnesCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time < endOfDay && x.MemberId.HasValue);
            var bigSmallTurboTotalBets = await bigSmallTurboOnesCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time < endOfDay && x.MemberId.HasValue);
            var evenOddTotalBets = await evenOddOnesCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time < endOfDay && x.MemberId.HasValue);
            var evenOddTurboTotalBets = await evenOddTurboOnesCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time < endOfDay && x.MemberId.HasValue);

            var fishPrawnCrabTotalBets = await fishPrawnCrabBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time < endOfDay && x.MemberId.HasValue);
            var shakeThePlateTotalBets = await shakeThePlateBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time < endOfDay && x.MemberId.HasValue);
            var dragonTigerTotalBets = await dragonTigerBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time < endOfDay && x.MemberId.HasValue);

            return bigSmallTotalBets + fishPrawnCrabTotalBets + evenOddTotalBets + evenOddTurboTotalBets + shakeThePlateTotalBets + bigSmallTurboTotalBets + dragonTigerTotalBets;
        }
    }
}