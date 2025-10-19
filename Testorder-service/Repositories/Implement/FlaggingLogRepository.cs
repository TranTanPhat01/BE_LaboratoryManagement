using Microsoft.EntityFrameworkCore;
using Testorder_service.Repositories.Interface;
using TestOrderService.Data;
using TestOrderService.Models;

namespace Testorder_service.Repositories.Implement
{
    public class FlaggingLogRepository : IFlaggingLogRepository
    {
        private readonly TestDbContext _db;
        public FlaggingLogRepository(TestDbContext db) => _db = db;

        public Task<flagging_config_log?> GetAsync(long id, CancellationToken ct) =>
            _db.flagging_config_logs.FirstOrDefaultAsync(x => x.id == id, ct);

        public Task<List<flagging_config_log>> ListByConfigAsync(long flagConfigId, CancellationToken ct) =>
            _db.flagging_config_logs
               .AsNoTracking()
               .Where(x => x.flag_config_id == flagConfigId)
               .OrderByDescending(x => x.logged_at)
               .ToListAsync(ct);

        public Task AddAsync(flagging_config_log entity, CancellationToken ct)
        {
            _db.flagging_config_logs.Add(entity);
            return Task.CompletedTask;
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
