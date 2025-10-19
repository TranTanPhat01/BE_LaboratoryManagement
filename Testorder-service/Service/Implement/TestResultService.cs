using AutoMapper;
using NodaTime;
using NodaTime.Text;
using Testorder_service.Models.Dto;
using Testorder_service.Repositories.Interface;
using Testorder_service.Service.Interface;
using TestOrderService.Models;

namespace Testorder_service.Service.Implement
{
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultRepository _repo;
        private readonly IMapper _mapper;
        private readonly IClock _clock;

        public TestResultService(ITestResultRepository repo, IMapper mapper, IClock? clock = null)
        {
            _repo = repo; _mapper = mapper; _clock = clock ?? SystemClock.Instance;
        }

        public async Task<long> CreateAsync(CreateTestResultDto dto, CancellationToken ct)
        {
            var e = _mapper.Map<test_result>(dto);
            e.created_at = _clock.GetCurrentInstant().InUtc().LocalDateTime;

            await _repo.AddAsync(e, ct);
            await _repo.SaveAsync(ct);
            return e.id;
        }

        public async Task UpdateAsync(UpdateTestResultDto dto, CancellationToken ct)
        {
            var e = await _repo.GetAsync(dto.Id, ct) ?? throw new KeyNotFoundException("Result not found");
            _mapper.Map(dto, e); // patch non-null
            await _repo.UpdateAsync(e, ct);
            await _repo.SaveAsync(ct);
        }

        public async Task<TestResultDto?> GetAsync(long id, CancellationToken ct)
        {
            var e = await _repo.GetAsync(id, ct);
            return e is null ? null : _mapper.Map<TestResultDto>(e);
        }

        public async Task<List<TestResultDto>> ListBySampleAsync(long sampleId, CancellationToken ct)
        {
            var list = await _repo.ListBySampleAsync(sampleId, ct);
            return _mapper.Map<List<TestResultDto>>(list);
        }
    }
}
