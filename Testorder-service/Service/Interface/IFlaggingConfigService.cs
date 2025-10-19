using Testorder_service.Models.Dto;

namespace Testorder_service.Service.Interface
{
    public interface IFlaggingConfigService
    {
        Task<FlaggingConfigDto> UpsertAsync(UpsertFlaggingConfigDto dto, CancellationToken ct);
        Task<FlaggingConfigDto?> GetByIdAsync(long id, CancellationToken ct);
        Task<FlaggingConfigDto?> GetByAnalyteAsync(string analyteCode, CancellationToken ct);
        Task<List<FlaggingConfigDto>> ListAsync(CancellationToken ct);
        Task SetActiveAsync(long id, bool active, string updatedBy, CancellationToken ct);
    }
}
