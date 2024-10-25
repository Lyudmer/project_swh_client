﻿using ClientSWH.Core.Models;


namespace ClientSWH.Core.Abstraction.Repositories
{
    public interface IDocumentsRepository
    {
        Task<Document> Add(Document Doc);
        Task Delete(int Id);
        Task<List<Document>> GetByFilter(int pid);
        Task<Document> GetById(int id);
        Task<List<Document>> GetByPage(int page, int page_size);
        Task Update(Guid DocId, Document Doc);
        Task<int> GetLastDocId();
    }
}