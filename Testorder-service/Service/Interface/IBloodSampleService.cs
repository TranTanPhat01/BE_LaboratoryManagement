using Testorder_service.Models.Dto;

namespace Testorder_service.Service.Interface
{
    public interface IBloodSampleService
    {
        Task<long> CreateAsync(CreateBloodSampleDto dto, CancellationToken ct);
        Task UpdateAsync(UpdateBloodSampleDto dto, CancellationToken ct);
        Task<BloodSampleDto?> GetAsync(long id, CancellationToken ct);
        Task<(int total, List<BloodSampleDto> items)> ListAsync(int page, int pageSize, CancellationToken ct);
    }
}
