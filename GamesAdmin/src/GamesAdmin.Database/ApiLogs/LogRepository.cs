using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GamesAdmin.Database.ApiLogs
{
    public interface ILogRepository<TDocument>
    {
        Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);

        void SetCollection(IMongoCollection<TDocument> collection);
    }

    public class LogRepository<TDocument> : ILogRepository<TDocument>
    {
        private IMongoCollection<TDocument> collection;

        public virtual async Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression)
        => (await collection.FindAsync(filterExpression)).ToEnumerable();

        public void SetCollection(IMongoCollection<TDocument> collection)
        {
            this.collection = this.collection ?? collection;
        }
    }
}