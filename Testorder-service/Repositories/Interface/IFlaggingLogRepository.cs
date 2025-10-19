using TestOrderService.Models;

namespace Testorder_service.Repositories.Interface
{
    public interface IFlaggingLogRepository
    {
        Task<flagging_config_log?> GetAsync(long id, CancellationToken ct);
        Task<List<flagging_config_log>> ListByConfigAsync(long flagConfigId, CancellationToken ct);
        Task AddAsync(flagging_config_log entity, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
