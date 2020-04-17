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
            var currentDate = DateTime.UtcNow.Date;

            var startOfDay = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var endOfDay = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59, DateTimeKind.Utc);

            var bigSmallBetCollection = database.GetCollection<BigSmallBetEntity>(Document.GetCollectionName(typeof(BigSmallBetEntity)));
            var fishPrawnCrabCollection = database.GetCollection<FishPrawnCrabBetEntity>(Document.GetCollectionName(typeof(FishPrawnCrabBetEntity)));
            var shakeThePlaceBetCollection = database.GetCollection<ShakeThePlateBetEntity>(Document.GetCollectionName(typeof(ShakeThePlateBetEntity)));
            var bigSmallTurboBetCollection = database.GetCollection<BigSmallTurboBetEntity>(Document.GetCollectionName(typeof(BigSmallTurboBetEntity)));

            var bigSmallTotalBets = await bigSmallBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);
            var fishPrawnCrabTotalBets = await fishPrawnCrabCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);
            var shakeThePlaceTotalBets = await shakeThePlaceBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);
            var bigSmallTurboTotalBets = await bigSmallTurboBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);


            return bigSmallTotalBets + fishPrawnCrabTotalBets + shakeThePlaceTotalBets + bigSmallTurboTotalBets;
        }
    }
}