using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Testorder_service.Service.Interface;

namespace Testorder_service.Controllers
{
    [Route("api/test/flagging/logs")]
    [ApiController]
    public class FlaggingLogsController : ControllerBase
    {
        private readonly IFlaggingLogService _svc;
        public FlaggingLogsController(IFlaggingLogService svc) => _svc = svc;

        [HttpGet("by-config/{configId:long}")]
        public async Task<IActionResult> ListByConfig(long configId, CancellationToken ct)
        {
            var list = await _svc.ListByConfigAsync(configId, ct);
            return Ok(list);
        }
    }
}
