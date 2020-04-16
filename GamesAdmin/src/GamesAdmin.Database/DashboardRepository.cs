using System;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IDashboardRepository
    {
        Task<long> GetTodayTotalBets();
    }

    public class DashboardRepository : IDashboardRepository
    {
        private readonly IMongoDatabase database;

        public DashboardRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<long> GetTodayTotalBets()
        {
            var currentDate = DateTime.UtcNow.Date;

            var startOfDay = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var endOfDay = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59, DateTimeKind.Utc);

            var bigSmallBetCollection = database.GetCollection<BigSmallBetEntity>(Document.GetCollectionName(typeof(BigSmallBetEntity)));

            var result = await bigSmallBetCollection.CountDocumentsAsync(x => x.Time >= startOfDay && x.Time <= endOfDay && x.MemberId.HasValue);


            return result;
        }
    }
}