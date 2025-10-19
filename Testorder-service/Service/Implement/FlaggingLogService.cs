using AutoMapper;
using NodaTime;
using NodaTime.Text;
using Testorder_service.Models.Dto;
using Testorder_service.Repositories.Interface;
using Testorder_service.Service.Interface;
using TestOrderService.Models;

namespace Testorder_service.Service.Implement
{
    public class FlaggingLogService : IFlaggingLogService
    {
        private readonly IFlaggingLogRepository _repo;
        private readonly IMapper _mapper;
        private readonly IClock _clock;

        public FlaggingLogService(IFlaggingLogRepository repo, IMapper mapper, IClock? clock = null)
        {
            _repo = repo; _mapper = mapper; _clock = clock ?? SystemClock.Instance;
        }

        public async Task<List<FlaggingLogDto>> ListByConfigAsync(long flagConfigId, CancellationToken ct)
            => _mapper.Map<List<FlaggingLogDto>>(await _repo.ListByConfigAsync(flagConfigId, ct));

        //public async Task<long> AddAsync(CreateFlaggingLogDto dto, CancellationToken ct)
        //{
        //    var e = _mapper.Map<flagging_config_log>(dto);
        //    e.logged_at = _clock.GetCurrentInstant().InUtc().LocalDateTime;

        //    await _repo.AddAsync(e, ct);
        //    await _repo.SaveAsync(ct);
        //    return e.id;
        //}
    }
}
