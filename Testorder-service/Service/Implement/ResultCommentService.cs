using AutoMapper;
using NodaTime;
using NodaTime.Text;
using Testorder_service.Models.Dto;
using Testorder_service.Repositories.Interface;
using Testorder_service.Service.Interface;
using TestOrderService.Models;

namespace Testorder_service.Service.Implement
{
    public class ResultCommentService : IResultCommentService
    {

        private readonly IResultCommentRepository _repo;
        private readonly IMapper _mapper;
        private readonly IClock _clock;

        public ResultCommentService(IResultCommentRepository repo, IMapper mapper, IClock? clock = null)
        {
            _repo = repo; _mapper = mapper; _clock = clock ?? SystemClock.Instance;
        }

        public async Task<long> UpsertAsync(UpsertResultCommentDto dto, CancellationToken ct)
        {
            var e = _mapper.Map<result_comment>(dto);
            e.commented_at = _clock.GetCurrentInstant().InUtc().LocalDateTime;

            await _repo.AddAsync(e, ct);
            await _repo.SaveAsync(ct);
            return e.id;
        }

        public async Task<List<ResultCommentDto>> ListByResultAsync(long resultId, CancellationToken ct)
            => _mapper.Map<List<ResultCommentDto>>(await _repo.ListByResultAsync(resultId, ct));

        public async Task<List<ResultCommentDto>> ListBySampleAsync(long sampleId, CancellationToken ct)
            => _mapper.Map<List<ResultCommentDto>>(await _repo.ListBySampleAsync(sampleId, ct));
    }
}
