using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Testorder_service.Models.Dto;
using Testorder_service.Service.Interface;

namespace Testorder_service.Controllers
{
    [Route("api/test/comments")]
    [ApiController]
    public class ResultCommentsController : ControllerBase
    {
        private readonly IResultCommentService _svc;
        public ResultCommentsController(IResultCommentService svc) => _svc = svc;

        [HttpGet("by-result/{resultId:long}")]
        public async Task<IActionResult> ListByResult(long resultId, CancellationToken ct)
        {
            var list = await _svc.ListByResultAsync(resultId, ct);
            return Ok(list);
        }

        [HttpGet("by-sample/{sampleId:long}")]
        public async Task<IActionResult> ListBySample(long sampleId, CancellationToken ct)
        {
            var list = await _svc.ListBySampleAsync(sampleId, ct);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] UpsertResultCommentDto dto, CancellationToken ct)
        {
            var id = await _svc.UpsertAsync(dto, ct);
            return Created(string.Empty, new { id });
        }
    }
}
