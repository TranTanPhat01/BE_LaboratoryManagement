using TestOrderService.Models;

namespace Testorder_service.Repositories.Interface
{
    public interface ITestResultRepository
    {
        Task<test_result?> GetAsync(long id, CancellationToken ct);
        Task<List<test_result>> ListBySampleAsync(long sampleId, CancellationToken ct);
        Task AddAsync(test_result entity, CancellationToken ct);
        Task UpdateAsync(test_result entity, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
