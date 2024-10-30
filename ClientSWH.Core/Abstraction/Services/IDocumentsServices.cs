using ClientSWH.Core.Models;
using ClientSWH.DocsRecordCore.Models;

namespace ClientSWH.Core.Abstraction.Services
{
    public interface IDocumentsServices
    {
        Task<Document> GetDocId(int Id);
        Task<DocRecordBase> GetDocRecord(int Id);
        Task<bool> DeleteDoc(int Id);
    }
}