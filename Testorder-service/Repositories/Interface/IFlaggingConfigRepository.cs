using TestOrderService.Models;

namespace Testorder_service.Repositories.Interface
{
    public interface IFlaggingConfigRepository
    {
        Task<flagging_configuration?> GetAsync(long id, CancellationToken ct);
        Task<flagging_configuration?> GetByAnalyteAsync(string analyteCode, CancellationToken ct);
        Task<List<flagging_configuration>> ListAsync(CancellationToken ct);
        Task AddAsync(flagging_configuration entity, CancellationToken ct);
        Task UpdateAsync(flagging_configuration entity, CancellationToken ct);
        Task UpsertAsync(flagging_configuration entity, CancellationToken ct); // tiện lợi
        Task SaveAsync(CancellationToken ct);
    }
}
