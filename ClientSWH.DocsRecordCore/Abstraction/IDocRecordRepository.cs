using ClientSWH.DocsRecordCore.Models;

namespace ClientSWH.DocsRecordCore.Abstraction
{
    public interface IDocRecordRepository
    {
        Task<string> AddRecord(DocRecord item);
        Task<long> DeleteId(string Docid);
        Task<DocRecord> GetByDocId(Guid docId);
        Task<long> UpdateRecord(Guid DocId, DocRecord docRecord);
    }
}