using Testorder_service.Models.Dto;

namespace Testorder_service.Service.Interface
{
    public interface IResultCommentService
    {
        Task<long> UpsertAsync(UpsertResultCommentDto dto, CancellationToken ct);
        Task<List<ResultCommentDto>> ListByResultAsync(long resultId, CancellationToken ct);
        Task<List<ResultCommentDto>> ListBySampleAsync(long sampleId, CancellationToken ct);
    }
}
