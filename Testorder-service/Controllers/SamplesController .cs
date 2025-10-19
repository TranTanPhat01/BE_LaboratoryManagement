using Microsoft.AspNetCore.Mvc;
using Testorder_service.Models.Dto;
using Testorder_service.Service.Interface;

namespace Testorder_service.Controllers
{
    [ApiController]
    [Route("api/test/bloodsamples")]
    public class SamplesController : ControllerBase
    {
        private readonly IBloodSampleService _svc;
        public SamplesController(IBloodSampleService svc) => _svc = svc;

        // GET /api/testorder/samples?page=1&pageSize=20
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var (total, items) = await _svc.ListAsync(page, pageSize, ct);
            return Ok(new { total, items, page, pageSize });
        }

        // GET /api/testorder/samples/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id, CancellationToken ct)
        {
            var item = await _svc.GetAsync(id, ct);
            return item is null ? NotFound() : Ok(item);
        }

        // POST /api/testorder/samples
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBloodSampleDto dto, CancellationToken ct)
        {
            var id = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        // PUT /api/testorder/samples/{id}
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateBloodSampleDto dto, CancellationToken ct)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");
            await _svc.UpdateAsync(dto, ct);
            return NoContent();
        }

    }
}
