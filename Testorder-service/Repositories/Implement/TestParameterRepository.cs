using Microsoft.EntityFrameworkCore;
using Testorder_service.Repositories.Interface;
using TestOrderService.Data;
using TestOrderService.Models;

namespace Testorder_service.Repositories.Implement
{
    public class TestParameterRepository : ITestParameterRepository
    {
        private readonly TestDbContext _db;
        public TestParameterRepository(TestDbContext db) => _db = db;

        public Task<test_parameter?> GetAsync(long id, CancellationToken ct) =>
            _db.test_parameters.FirstOrDefaultAsync(p => p.id == id, ct);

        public Task<List<test_parameter>> ListByResultAsync(long resultId, CancellationToken ct) =>
            _db.test_parameters
               .AsNoTracking()
               .Where(p => p.test_result_id == resultId)
               .OrderBy(p => p.param_name)
               .ToListAsync(ct);

        public Task AddAsync(test_parameter entity, CancellationToken ct)
        {
            _db.test_parameters.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(test_parameter entity, CancellationToken ct)
        {
            _db.test_parameters.Update(entity);
            return Task.CompletedTask;
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
