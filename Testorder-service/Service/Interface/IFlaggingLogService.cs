using Testorder_service.Models.Dto;

namespace Testorder_service.Service.Interface
{
    public interface IFlaggingLogService
    {
        Task<List<FlaggingLogDto>> ListByConfigAsync(long flagConfigId, CancellationToken ct);
      
    }
}
