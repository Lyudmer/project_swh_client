
using ClientSWH.Contracts;
using ClientSWH.Core.Abstraction.Services;
using ClientSWH.Core.Models;

using Microsoft.AspNetCore.Mvc;

namespace ClientSWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController(IStatusServices statusService) : ControllerBase
    {
        private readonly IStatusServices _statusService = statusService;

        [HttpPost("AddStatus")]
        public async Task<IActionResult> AddStatus([FromBody] StatusAddRequest stRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result= await _statusService.AddStatus(
                                            Status.Create(stRequest.Id, stRequest.StatusName, stRequest.RunWf, stRequest.MkRes, stRequest.SendMess)
                                            );
            return Ok(result);
        }
        [HttpPost("DelStatus")]
        public async Task<IActionResult> DelStatus([FromBody] StatusDelRequest statusRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           var result = await _statusService.DelStatus(statusRequest.Id);
            return Ok(result);
        }
    }
}
