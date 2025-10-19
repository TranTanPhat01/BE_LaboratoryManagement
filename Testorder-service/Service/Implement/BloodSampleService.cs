using AutoMapper;
using NodaTime;
using NodaTime.Text;
using Testorder_service.Models.Dto;
using Testorder_service.Repositories.Interface;
using Testorder_service.Service.Interface;
using TestOrderService.Models;

namespace Testorder_service.Service.Implement
{
    public class BloodSampleService : IBloodSampleService
    {
        private readonly IBloodSampleRepository _repo;
        private readonly IMapper _mapper;
        private readonly IClock _clock;

        public BloodSampleService(IBloodSampleRepository repo, IMapper mapper, IClock? clock = null)
        {
            _repo = repo;
            _mapper = mapper;
            _clock = clock ?? SystemClock.Instance;
        }

        public async Task<long> CreateAsync(CreateBloodSampleDto dto, CancellationToken ct)
        {
            var e = _mapper.Map<blood_sample>(dto);
            e.created_at = _clock.GetCurrentInstant().InUtc().LocalDateTime;

            await _repo.AddAsync(e, ct);
            await _repo.SaveAsync(ct);
            return e.id;
        }

        public async Task UpdateAsync(UpdateBloodSampleDto dto, CancellationToken ct)
        {
            var e = await _repo.GetAsync(dto.Id, ct) ?? throw new KeyNotFoundException("Sample not found");

            _mapper.Map(dto, e); // patch non-null
            e.updated_at = _clock.GetCurrentInstant().InUtc().LocalDateTime;

            await _repo.UpdateAsync(e, ct);
            await _repo.SaveAsync(ct);
        }

        public async Task<BloodSampleDto?> GetAsync(long id, CancellationToken ct)
        {
            var e = await _repo.GetAsync(id, ct);
            return e is null ? null : _mapper.Map<BloodSampleDto>(e);
        }

        public async Task<(int total, List<BloodSampleDto> items)> ListAsync(int page, int pageSize, CancellationToken ct)
        {
            var total = await _repo.CountAsync(ct);
            var list = await _repo.ListAsync(page, pageSize, ct);
            return (total, _mapper.Map<List<BloodSampleDto>>(list));
        }
    }
}

