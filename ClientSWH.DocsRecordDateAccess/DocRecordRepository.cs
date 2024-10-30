
using ClientSWH.DocsRecordCore.Abstraction;
using ClientSWH.DocsRecordCore.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System;
using static System.Collections.Specialized.BitVector32;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace ClientSWH.DocsRecordDataAccess
{
    public class DocRecordRepository(DocRecordContext mongocontext) : IDocRecordRepository
    {

        private readonly DocRecordContext _mongocontext = mongocontext;
        
        public async Task<DocRecordBase> GetByDocId(string docId)
        {
            try
            {
                var query = await _mongocontext.DocRecords
                                .Find(x => x.DocId.Contains(docId))
                                .FirstOrDefaultAsync();
                var resRec = new DocRecordBase
                {
                    DocText = query.DocText,
                    DocId = query.DocId
                };

                if (resRec == null) return null;
                else return resRec;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> GetDocTextDocId(string docId)
        {
            try
            {

                var query = await _mongocontext.DocRecords
                                .Find(x => x.DocId.Contains(docId))
                                .FirstOrDefaultAsync();

               var resDocText= query.DocText;

                if (resDocText == string.Empty) return string.Empty;
                else return resDocText;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> AddRecord(string docId, string doctext)
        {
            try
            {
                var item = new DocRecord( docId , doctext );
                await _mongocontext.DocRecords.InsertOneAsync(item);
                var resRec = await _mongocontext.DocRecords
                                 .Find(x => x.DocId == docId)
                                 .FirstOrDefaultAsync();
                if (resRec == null) return string.Empty;
                else return resRec.Id.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public async Task<long> UpdateRecord(DocRecord item)
        {
            var filter = Builders<DocRecord>.Filter.Eq(s => s.DocId, item.DocId);
            var update = Builders<DocRecord>.Update.Set(s => s.DocText, item.DocText);
            try
            {
                var resUpdate = await _mongocontext.DocRecords.UpdateOneAsync(filter, update);
                if (resUpdate != null) return resUpdate.ModifiedCount;
                else return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<long> DeleteId(string Docid)
        {
            try
            {
                var resDel = await _mongocontext.DocRecords.DeleteOneAsync(x => x.DocId == Docid);
                if (resDel != null) return resDel.DeletedCount;
                else return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task CreateRecord(DocRecord item)
        {
            await _mongocontext.DocRecords.InsertOneAsync(item);
        }
        public async Task<IEnumerable<DocRecord>> GetRecords()
        {
            return await _mongocontext.DocRecords.Find(x => true).ToListAsync();
        }
    }
}
