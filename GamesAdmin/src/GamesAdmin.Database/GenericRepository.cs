using MongoDbGenericRepository;

namespace GamesAdmin.Database
{
    public class GenericRepository : BaseMongoRepository
    {
        public GenericRepository(string connectionString, string databaseName) 
            : base(connectionString, databaseName)
        {
            MongoDbContext.SetGuidRepresentation(MongoDB.Bson.GuidRepresentation.CSharpLegacy);
        }
    }
}
