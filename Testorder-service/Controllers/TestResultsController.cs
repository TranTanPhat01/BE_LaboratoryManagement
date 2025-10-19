using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Testorder_service.Models.Dto;
using Testorder_service.Service.Interface;

namespace Testorder_service.Controllers
{
    [Route("api/test/results")]
    [ApiController]
    public class TestResultsController : ControllerBase
    {
        private readonly ITestResultService _svc;
        public TestResultsController(ITestResultService svc) => _svc = svc;


        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id, CancellationToken ct)
        {
            var r = await _svc.GetAsync(id, ct);
            return r is null ? NotFound() : Ok(r);
        }

        [HttpGet("by-sample/{sampleId:long}")]
        public async Task<IActionResult> ListBySample(long sampleId, CancellationToken ct)
        {
            var list = await _svc.ListBySampleAsync(sampleId, ct);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestResultDto dto, CancellationToken ct)
        {
            var id = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateTestResultDto dto, CancellationToken ct)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");
            await _svc.UpdateAsync(dto, ct);
            return NoContent();
        }
    }
}
