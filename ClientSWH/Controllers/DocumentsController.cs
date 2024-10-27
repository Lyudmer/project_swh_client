﻿

using ClientSWH.Contracts;
using ClientSWH.Core.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;


namespace ClientSWH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController(IDocumentsServices docService) : ControllerBase
    {
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
                
            var result = await _docService.GetDocRecord(docR.Id);
            if (result == null)
                return Ok("Документ не найден");
            else return Ok(result);
        }
        [HttpPost("DeleteDoc")]
        public async Task<IActionResult> DeleteDoc(DocRequest docR)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _docService.DeleteDoc(docR.Id);

            return Ok();
        }
    }
}
