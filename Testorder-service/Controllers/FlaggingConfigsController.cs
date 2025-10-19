using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Testorder_service.Models.Dto;
using Testorder_service.Service.Interface;

namespace Testorder_service.Controllers
{
    [Route("api/test/flagging/configs")]
    [ApiController]
    public class FlaggingConfigsController : ControllerBase
    {
        private readonly IFlaggingConfigService _svc;
        public FlaggingConfigsController(IFlaggingConfigService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> List(CancellationToken ct)
            => Ok(await _svc.ListAsync(ct));

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id, CancellationToken ct)
        {
            var cfg = await _svc.GetByIdAsync(id, ct);
            return cfg is null ? NotFound() : Ok(cfg);
        }

        [HttpGet("by-analyte/{code}")]
        public async Task<IActionResult> GetByAnalyte(string code, CancellationToken ct)
        {
            var cfg = await _svc.GetByAnalyteAsync(code, ct);
            return cfg is null ? NotFound() : Ok(cfg);
        }

        // Upsert
        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] UpsertFlaggingConfigDto dto, CancellationToken ct)
        {
            var saved = await _svc.UpsertAsync(dto, ct);
            return Ok(saved);
        }

        [HttpPut("{id:long}/set-active")]
        public async Task<IActionResult> SetActive(long id, [FromQuery] bool active, [FromQuery] string updatedBy, CancellationToken ct)
        {
            await _svc.SetActiveAsync(id, active, updatedBy, ct);
            return NoContent();
        }
    }
}
