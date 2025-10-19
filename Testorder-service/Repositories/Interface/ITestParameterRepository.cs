using TestOrderService.Models;

namespace Testorder_service.Repositories.Interface
{
    public interface ITestParameterRepository
    {
        Task<test_parameter?> GetAsync(long id, CancellationToken ct);
        Task<List<test_parameter>> ListByResultAsync(long resultId, CancellationToken ct);
        Task AddAsync(test_parameter entity, CancellationToken ct);
        Task UpdateAsync(test_parameter entity, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
