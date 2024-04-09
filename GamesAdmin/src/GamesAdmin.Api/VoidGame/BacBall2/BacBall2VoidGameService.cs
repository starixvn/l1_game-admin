using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.VoidGame.BacBall2
{
    public interface IBacBall2VoidGameService
    {      

        Task<IList<T>> FindTicket<T>(
              IMongoCollection<T> collection,
              string obCustId,
              string partnerId,
              string siteId,
              long gameRoundId,
              bool freeBet) where T : BaseBetEntity;

        Task<DeleteResult> RemoveTicket<T>(
                IMongoCollection<T> collection,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet)
            where T : BaseBetEntity;
    }

    public class BacBall2VoidGameService : IBacBall2VoidGameService
    {
        public async Task<IList<T>> FindTicket<T>(
               IMongoCollection<T> collection,
               string obCustId,
               string partnerId,
               string siteId,
               long gameRoundId,
               bool freeBet)
           where T : BaseBetEntity
        {
            var record = (await collection
                .FindAsync(record => record.PlayerId == obCustId
                                    && record.OperatorId == partnerId
                                    && record.SiteId == siteId
                                    && (record.DrawId == gameRoundId)
                                    && (!freeBet || record.FreeBet == freeBet)))
                .ToList();

            return record;
        }

        public async Task<DeleteResult> RemoveTicket<T>(
                IMongoCollection<T> collection,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet)
            where T : BaseBetEntity
        {
            var result = (await collection
                .DeleteManyAsync(record => record.PlayerId == obCustId
                                    && record.SiteId == siteId
                                    && record.OperatorId == partnerId
                                    && (record.DrawId == gameRoundId)
                                    && (!freeBet || record.FreeBet == freeBet)));

            return result;
        }
    }
}
