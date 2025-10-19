using TestOrderService.Models;

namespace Testorder_service.Repositories.Interface
{
    public interface IBloodSampleRepository
    {
        Task<blood_sample?> GetAsync(long id, CancellationToken ct);
        Task AddAsync(blood_sample entity, CancellationToken ct);
        Task UpdateAsync(blood_sample entity, CancellationToken ct);
        Task<int> CountAsync(CancellationToken ct);
        Task<List<blood_sample>> ListAsync(int page, int pageSize, CancellationToken ct);
        IQueryable<blood_sample> Query();   // cho filter nâng cao nếu cần
        Task SaveAsync(CancellationToken ct);
    }
}
