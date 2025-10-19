using Testorder_service.Models.Dto;

namespace Testorder_service.Service.Interface
{
    public interface ITestParameterService
    {
        Task<long> CreateAsync(CreateTestParameterDto dto, CancellationToken ct);
        Task UpdateAsync(UpdateTestParameterDto dto, CancellationToken ct);
        Task<List<TestParameterDto>> ListByResultAsync(long resultId, CancellationToken ct);
    }
}

