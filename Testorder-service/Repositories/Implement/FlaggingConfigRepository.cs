using Microsoft.EntityFrameworkCore;
using Testorder_service.Repositories.Interface;
using TestOrderService.Data;
using TestOrderService.Models;

namespace Testorder_service.Repositories.Implement
{
    public class FlaggingConfigRepository : IFlaggingConfigRepository
    {
        private readonly TestDbContext _db;
        public FlaggingConfigRepository(TestDbContext db) => _db = db;

        public Task<flagging_configuration?> GetAsync(long id, CancellationToken ct) =>
           _db.flagging_configurations.FirstOrDefaultAsync(x => x.id == id, ct);

        public Task<flagging_configuration?> GetByAnalyteAsync(string analyteCode, CancellationToken ct) =>
            _db.flagging_configurations.FirstOrDefaultAsync(x => x.analyte_code == analyteCode, ct);

        public Task<List<flagging_configuration>> ListAsync(CancellationToken ct) =>
            _db.flagging_configurations
               .AsNoTracking()
               .OrderBy(x => x.analyte_code)
               .ToListAsync(ct);

        public Task AddAsync(flagging_configuration entity, CancellationToken ct)
        {
            _db.flagging_configurations.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(flagging_configuration entity, CancellationToken ct)
        {
            _db.flagging_configurations.Update(entity);
            return Task.CompletedTask;
        }

        public async Task UpsertAsync(flagging_configuration entity, CancellationToken ct)
        {
            var existed = await GetByAnalyteAsync(entity.analyte_code, ct);
            if (existed is null)
            {
                _db.flagging_configurations.Add(entity);
            }
            else
            {
                existed.analyte_name = entity.analyte_name;
                existed.normal_min = entity.normal_min;
                existed.normal_max = entity.normal_max;
                existed.critical_min = entity.critical_min;
                existed.critical_max = entity.critical_max;
                existed.unit = entity.unit;
                existed.flag_type = entity.flag_type;
                existed.version = entity.version;
                existed.updated_by = entity.updated_by;
                existed.updated_at = entity.updated_at;
                existed.active = entity.active;
                _db.flagging_configurations.Update(existed);
            }
            await _db.SaveChangesAsync(ct);
        }

        public Task SaveAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
