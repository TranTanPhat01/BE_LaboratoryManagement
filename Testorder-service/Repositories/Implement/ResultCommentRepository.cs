using Microsoft.EntityFrameworkCore;
using Testorder_service.Repositories.Interface;
using TestOrderService.Data;
using TestOrderService.Models;

namespace Testorder_service.Repositories.Implement
{
    public class ResultCommentRepository : IResultCommentRepository
    {
        private readonly TestDbContext _db;
        public ResultCommentRepository(TestDbContext db) => _db = db;

        public Task<result_comment?> GetAsync(long id, CancellationToken ct) =>
            _db.result_comments.FirstOrDefaultAsync(x => x.id == id, ct);

        public Task<List<result_comment>> ListByResultAsync(long resultId, CancellationToken ct) =>
            _db.result_comments
               .AsNoTracking()
               .Where(x => x.result_id == resultId)
               .OrderByDescending(x => x.commented_at)
               .ToListAsync(ct);

        public Task<List<result_comment>> ListBySampleAsync(long sampleId, CancellationToken ct) =>
            _db.result_comments
               .AsNoTracking()
               .Where(x => x.sample_id == sampleId)
               .OrderByDescending(x => x.commented_at)
               .ToListAsync(ct);

        public Task AddAsync(result_comment entity, CancellationToken ct)
        {
            _db.result_comments.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(result_comment entity, CancellationToken ct)
        {
            _db.result_comments.Update(entity);
            return Task.CompletedTask;
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
