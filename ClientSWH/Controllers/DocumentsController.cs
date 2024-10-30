

using ClientSWH.Contracts;
using ClientSWH.Core.Abstraction.Services;
using ClientSWH.DocsRecordCore.Abstraction;
using ClientSWH.DocsRecordCore.Models;
using ClientSWH.DocsRecordDataAccess;
using Microsoft.AspNetCore.Mvc;


namespace ClientSWH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController(IDocumentsServices docService, IDocRecordRepository mongo) : ControllerBase
    {
        private readonly IDocRecordRepository _mongo = mongo;
        private readonly IDocumentsServices _docService = docService;

        [HttpPost("GetDocument")]
        public async Task<IActionResult> GetDocId(DocRequest docR)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _docService.GetDocId(docR.Id);
            if (result == null)
                return Ok("Документ не найден");
            else return Ok(result);
        } 
        [HttpPost("GetDocRecord")]
        public async Task<IActionResult> GetDocRecId(DocRequest docR)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Doc = await _docService.GetDocId(docR.Id);
            if (Doc != null)
            {
                var response = new DocRecordResponseDocId();
                response.MongoDocRecord= await _mongo.GetByDocId(Doc.DocId.ToString().Trim());
                if(response == null)
                return Ok(Doc);
                else 
                    return Ok(response);
            }
            return Ok("Документ не найден");
        }
        [HttpPost("DeleteDoc")]
        public async Task<IActionResult> DeleteDoc(DocRequest docR)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _docService.DeleteDoc(docR.Id);

            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateDiscountCoupon(DocRecord item)
        //{

        //    await _mongo.CreateRecord(item);
        //    return Ok();
        //}
        [HttpGet]
        public async Task<IActionResult> GetDocRecords()
        {
            var response = new DocRecordResponse();

            response.MongoDocRecord = await _mongo.GetRecords();

            return Ok(response);
        }
        
    }
}
