using TestOrderService.Models;

namespace Testorder_service.Repositories.Interface
{
    public interface IResultCommentRepository
    {
        Task<result_comment?> GetAsync(long id, CancellationToken ct);
        Task<List<result_comment>> ListByResultAsync(long resultId, CancellationToken ct);
        Task<List<result_comment>> ListBySampleAsync(long sampleId, CancellationToken ct);
        Task AddAsync(result_comment entity, CancellationToken ct);
        Task UpdateAsync(result_comment entity, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
