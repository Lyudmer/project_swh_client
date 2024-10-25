using Microsoft.EntityFrameworkCore;
using ClientSWH.Core.Models;
using ClientSWH.Core.Abstraction.Repositories;
using ClientSWH.DataAccess.Entities;
using AutoMapper;

namespace ClientSWH.DataAccess.Repositories
{
    public class DocumentsRepository(ClientSWHDbContext dbContext, IMapper mapper) : IDocumentsRepository
    {
        private readonly ClientSWHDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        public async Task<Document> Add(Document Doc)
        {
            var docEntity = _mapper.Map<DocumentEntity>(Doc);
            var resEntity = await _dbContext.AddAsync(docEntity);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Document>(resEntity.Entity);
        }
        public async Task<Document> GetById(int id)
        {
            var docEntity = await _dbContext.Document
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);
            if (docEntity!= null) 
                return _mapper.Map<Document>(docEntity);
            else return null;    

        }
        public async Task<List<Document>> GetByFilter(int pid)
        {
            var query = _dbContext.Document.AsNoTracking();

            if (pid > 0) { query = query.Where(p => p.Pid == pid); }

            var docs = await query.ToListAsync();
            return _mapper.Map<List<Document>>(docs);

        }
        public async Task<List<Document>> GetByPage(int page, int page_size)
        {
            var query = _dbContext.Document
                .AsNoTracking()
                .Skip((page - 1) * page_size)
                .Take(page_size);

            var docs = await query.ToListAsync();
            return _mapper.Map<List<Document>>(docs);
        }

        public async Task Update(Guid DocId, Document Doc)
        {
            await _dbContext.Document
                .Where(p => p.DocId == DocId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.ModifyDate, DateTime.Now)
                                          .SetProperty(p => p.SizeDoc, Doc.SizeDoc)
                                          .SetProperty(p => p.DocType, Doc.DocType)
                                          .SetProperty(p => p.Idmd5, Doc.Idmd5)
                                          .SetProperty(p => p.IdSha256, Doc.IdSha256)
                                    );
        }
        public async Task Delete(int Id)
        {
            await _dbContext.Document
                .Where(u => u.Id == Id)
                .ExecuteDeleteAsync();
        }
        public async Task Delete(Guid Id)
        {
            await _dbContext.Document
                .Where(u => u.DocId == Id)
                .ExecuteDeleteAsync();
        }
        public async Task<int> GetLastDocId()
        {
            int cDoc = 0;
            try
            {
                var resId = await _dbContext.Document.MaxAsync(d=>d.Id);

                if (resId != 0) cDoc = resId;
                else cDoc = 0;

            }
            catch (Exception)
            {
                return cDoc;
            }
            return cDoc;

        }

       
    }
}
