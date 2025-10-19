using Microsoft.EntityFrameworkCore;
using Testorder_service.Repositories.Interface;
using TestOrderService.Data;
using TestOrderService.Models;

namespace Testorder_service.Repositories.Implement
{
    public class BloodSampleRepository : IBloodSampleRepository
    {
        private readonly TestDbContext _db;
        public BloodSampleRepository(TestDbContext db) => _db = db;

        public Task<blood_sample?> GetAsync(long id, CancellationToken ct) =>
            _db.blood_samples
               .Include(s => s.test_results)
               .ThenInclude(r => r.test_parameters)
               .Include(s => s.result_comments)
               .FirstOrDefaultAsync(s => s.id == id, ct);

        public Task<int> CountAsync(CancellationToken ct) =>
            _db.blood_samples.CountAsync(ct);

        public async Task<List<blood_sample>> ListAsync(int page, int pageSize, CancellationToken ct)
        {
            page = page <= 0 ? 1 : page; pageSize = pageSize <= 0 ? 20 : pageSize;
            return await _db.blood_samples
                .AsNoTracking()
                .OrderByDescending(x => x.created_at)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public Task AddAsync(blood_sample entity, CancellationToken ct)
        {
            _db.blood_samples.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(blood_sample entity, CancellationToken ct)
        {
            _db.blood_samples.Update(entity);
            return Task.CompletedTask;
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

        public IQueryable<blood_sample> Query() => _db.blood_samples.AsQueryable();
    }
}

