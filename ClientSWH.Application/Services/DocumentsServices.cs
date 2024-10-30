
using ClientSWH.Core.Abstraction.Repositories;
using ClientSWH.Core.Abstraction.Services;
using ClientSWH.Core.Models;
using ClientSWH.DocsRecordCore.Abstraction;
using ClientSWH.DocsRecordCore.Models;
using Microsoft.AspNetCore.Mvc;




namespace ClientSWH.Application.Services
{
    public class DocumentsServices(IDocumentsRepository documentsRepository,
                                   IDocRecordRepository docRecordRepository) : IDocumentsServices
    {
        private readonly IDocumentsRepository _documentsRepository = documentsRepository;
        private readonly IDocRecordRepository _docRecordRepository = docRecordRepository;
        public async Task<Document> GetDocId(int Id)
        {
            return await _documentsRepository.GetById(Id);
        }
        public async Task<DocRecordBase> GetDocRecord(int Id)
        {
           var rDoc = await _documentsRepository.GetById(Id);
            if (rDoc == null) { return null; }
            else
            {
                var Rec = await _docRecordRepository.GetByDocId(rDoc.DocId.ToString());
                if (Rec == null) { return null; }
                else return Rec;
            }
        }
       
        public async Task<bool> DeleteDoc(int Id)
        {
            try
            {
                var rDoc = await _documentsRepository.GetById(Id);
                if (rDoc != null)
                {
                    var dRecord = await _docRecordRepository.GetByDocId(rDoc.DocId.ToString());
                    if (dRecord != null)
                                await _docRecordRepository.DeleteId(dRecord.DocId);
                    await _documentsRepository.Delete(Id);
                }
                return true;
            }
            catch
            { 
                return false;
            }
            
        }

        
    }
}
