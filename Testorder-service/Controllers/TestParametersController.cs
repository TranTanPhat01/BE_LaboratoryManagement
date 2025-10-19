using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Testorder_service.Models.Dto;
using Testorder_service.Service.Interface;

namespace Testorder_service.Controllers
{
    [Route("api/test/parameters")]
    [ApiController]
    public class TestParametersController : ControllerBase
    {
        private readonly ITestParameterService _svc;
        public TestParametersController(ITestParameterService svc) => _svc = svc;

        [HttpGet("by-result/{resultId:long}")]
        public async Task<IActionResult> ListByResult(long resultId, CancellationToken ct)
        {
            var list = await _svc.ListByResultAsync(resultId, ct);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestParameterDto dto, CancellationToken ct)
        {
            var id = await _svc.CreateAsync(dto, ct);
            return Created(string.Empty, new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateTestParameterDto dto, CancellationToken ct)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");
            await _svc.UpdateAsync(dto, ct);
            return NoContent();
        }
    }
}
