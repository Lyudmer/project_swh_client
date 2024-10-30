using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClientSWH.DocsRecordCore.Models
{
    public class DocRecord : DocRecordBase
    {
        public DocRecord(string docId, string doctext)
        {
            Id= ObjectId.GenerateNewId();
            DocId = docId;
            DocText = doctext;
        }

        [BsonId]
        public ObjectId Id { get; set; }
    }



}
