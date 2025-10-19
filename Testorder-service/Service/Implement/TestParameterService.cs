using AutoMapper;
using NodaTime;
using NodaTime.Text;
using Testorder_service.Models.Dto;
using Testorder_service.Repositories.Interface;
using Testorder_service.Service.Interface;
using TestOrderService.Models;

namespace Testorder_service.Service.Implement
{
    public class TestParameterService : ITestParameterService
    {
        private readonly ITestParameterRepository _repo;
        private readonly IMapper _mapper;

        public TestParameterService(ITestParameterRepository repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        public async Task<long> CreateAsync(CreateTestParameterDto dto, CancellationToken ct)
        {
            var e = _mapper.Map<test_parameter>(dto);
            await _repo.AddAsync(e, ct);
            await _repo.SaveAsync(ct);
            return e.id;
        }

        public async Task UpdateAsync(UpdateTestParameterDto dto, CancellationToken ct)
        {
            var e = await _repo.GetAsync(dto.Id, ct) ?? throw new KeyNotFoundException("Parameter not found");
            _mapper.Map(dto, e); // patch
            await _repo.UpdateAsync(e, ct);
            await _repo.SaveAsync(ct);
        }

        public async Task<List<TestParameterDto>> ListByResultAsync(long resultId, CancellationToken ct)
        {
            var list = await _repo.ListByResultAsync(resultId, ct);
            return _mapper.Map<List<TestParameterDto>>(list);
        }
    }
}
