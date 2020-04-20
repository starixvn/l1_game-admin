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
            var endOfDay = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59, DateTimeKind.Local).ToUniversalTime();

            var bigSmallOnesCollection = database.GetCollection<BigSmallOnesEntity>(Document.GetCollectionName(typeof(BigSmallOnesEntity)));
            var bigSmallTurboOnesCollection = database.GetCollection<BigSmallTurboOnesEntity>(Document.GetCollectionName(typeof(BigSmallTurboOnesEntity)));
            var evenOddOnesCollection = database.GetCollection<OddEvenOnesEntity>(Document.GetCollectionName(typeof(OddEvenOnesEntity)));

            var fishPrawnCrabBetCollection = database.GetCollection<FishPrawnCrabBetEntity>(Document.GetCollectionName(typeof(FishPrawnCrabBetEntity)));
            var shakeThePlaceBetCollection = database.GetCollection<ShakeThePlateBetEntity>(Document.GetCollectionName(typeof(ShakeThePlateBetEntity)));

            var bigSmallTotalBets = await bigSmallOnesCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);
            var bigSmallTurboTotalBets = await bigSmallTurboOnesCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);
            var evenOddTotalBets = await evenOddOnesCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);

            var fishPrawnCrabTotalBets = await fishPrawnCrabBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);
            var shakeThePlaceTotalBets = await shakeThePlaceBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);


            return bigSmallTotalBets + fishPrawnCrabTotalBets + evenOddTotalBets + shakeThePlaceTotalBets + bigSmallTurboTotalBets;
        }
    }
}