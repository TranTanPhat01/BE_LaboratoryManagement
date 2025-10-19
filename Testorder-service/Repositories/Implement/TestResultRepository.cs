using Microsoft.EntityFrameworkCore;
using Testorder_service.Repositories.Interface;
using TestOrderService.Data;
using TestOrderService.Models;

namespace Testorder_service.Repositories.Implement
{
    public class TestResultRepository : ITestResultRepository
    {
        private readonly TestDbContext _db;
        public TestResultRepository(TestDbContext db) => _db = db;

        public Task<test_result?> GetAsync(long id, CancellationToken ct) =>
           _db.test_results
              .Include(r => r.test_parameters)
              .Include(r => r.result_comments)
              .FirstOrDefaultAsync(r => r.id == id, ct);

        public Task<List<test_result>> ListBySampleAsync(long sampleId, CancellationToken ct) =>
            _db.test_results
               .AsNoTracking()
               .Where(r => r.sample_id == sampleId)
               .OrderByDescending(r => r.created_at)
               .ToListAsync(ct);

        public Task AddAsync(test_result entity, CancellationToken ct)
        {
            _db.test_results.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(test_result entity, CancellationToken ct)
        {
            _db.test_results.Update(entity);
            return Task.CompletedTask;
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}

