using ClientSWH.DocsRecordCore.Models;

namespace ClientSWH.DocsRecordCore.Abstraction
{
    public interface IDocRecordRepository
    {
        Task<string> AddRecord(string docId, string doctext);
        Task<long> DeleteId(string Docid);
        Task<DocRecordBase> GetByDocId(string docId);
        Task<string> GetDocTextDocId(string docId);
        Task<long> UpdateRecord(DocRecord item);
        Task CreateRecord(DocRecord item);
        Task<IEnumerable<DocRecord>> GetRecords();
    }
}