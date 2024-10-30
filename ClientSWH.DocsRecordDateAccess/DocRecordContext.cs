
using ClientSWH.DocsRecordCore.Models;
using MongoDB.Driver;

namespace ClientSWH.DocsRecordDataAccess
{
    public class DocRecordContext 
    {
        public IMongoClient _mongoClient;
        public IMongoDatabase _mongoDatabase;
        public IMongoCollection<DocRecord> DocRecords { get; set; }
        public DocRecordContext(IMongoClient _client)
        {
            _mongoClient = _client;
            _mongoDatabase = _mongoClient.GetDatabase("DocRecordSvh");
            DocRecords = _mongoDatabase.GetCollection<DocRecord>("DocRecord");
        }

    }
}
