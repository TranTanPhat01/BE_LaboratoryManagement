using Testorder_service.Models.Dto;

namespace Testorder_service.Service.Interface
{
    public interface ITestResultService
    {
        Task<long> CreateAsync(CreateTestResultDto dto, CancellationToken ct);
        Task UpdateAsync(UpdateTestResultDto dto, CancellationToken ct);
        Task<TestResultDto?> GetAsync(long id, CancellationToken ct);
        Task<List<TestResultDto>> ListBySampleAsync(long sampleId, CancellationToken ct);
    }
}
