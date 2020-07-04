using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using MongoDB.Driver;

namespace GamesAdmin.Site.DataProtection
{
    public class MongoDbXmlRepository : IXmlRepository
    {
        private readonly string appName;
        private readonly IMongoCollection<KeyElement> collection;

        public MongoDbXmlRepository(IMongoDatabase database, string collectionName, string appName)
        {
            collection = database.GetCollection<KeyElement>(collectionName);
            this.appName = appName;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var keyElements = collection.Find(x => x.AppName == appName).ToList();
            return keyElements.Select(element => XElement.Parse(element.Xml)).ToList().AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var keyElement = new KeyElement
            {
                AppName = appName,
                Xml = element.ToString(SaveOptions.DisableFormatting)
            };
            collection.InsertOne(keyElement);
        }
    }
}