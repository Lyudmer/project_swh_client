using ClientSWH.DocsRecordCore.Models;
using MongoDB.Driver;

namespace ClientSWH.DocsRecordCore.Abstraction
{
    public interface IDocRecordContext
    {
        IMongoCollection<DocRecord> DocRecords { get; }
    }
}