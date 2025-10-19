using AutoMapper;
using NodaTime;
using NodaTime.Text;
using System.Text.Json;
using Testorder_service.Models.Dto;
using Testorder_service.Repositories.Interface;
using Testorder_service.Service.Interface;
using TestOrderService.Models;

namespace Testorder_service.Service.Implement
{
    public class FlaggingConfigService : IFlaggingConfigService
    {
        private readonly IFlaggingConfigRepository _configs;
        private readonly IFlaggingLogRepository _logs;
        private readonly IMapper _mapper;
        private readonly IClock _clock;

        public FlaggingConfigService(IFlaggingConfigRepository configs,
                                     IFlaggingLogRepository logs,
                                     IMapper mapper,
                                     IClock? clock = null)
        {
            _configs = configs; _logs = logs; _mapper = mapper; _clock = clock ?? SystemClock.Instance;
        }

        public async Task<FlaggingConfigDto> UpsertAsync(UpsertFlaggingConfigDto dto, CancellationToken ct)
        {
            var now = _clock.GetCurrentInstant().InUtc().LocalDateTime;
            var existed = await _configs.GetByAnalyteAsync(dto.AnalyteCode, ct);

            if (existed is null)
            {
                var e = _mapper.Map<flagging_configuration>(dto);
                e.updated_at = now; // nếu có created_at trong entity, set thêm
                await _configs.AddAsync(e, ct);
                await _configs.SaveAsync(ct);

                await _logs.AddAsync(new flagging_config_log
                {
                    flag_config_id = e.id,
                    action = "CREATE",
                    new_data = System.Text.Json.JsonSerializer.Serialize(dto),
                    logged_at = now
                }, ct);
                await _logs.SaveAsync(ct);

                return _mapper.Map<FlaggingConfigDto>(e);
            }
            else
            {
                var oldJson = System.Text.Json.JsonSerializer.Serialize(existed);
                _mapper.Map(dto, existed); // patch
                existed.updated_at = now;

                await _configs.UpdateAsync(existed, ct);
                await _configs.SaveAsync(ct);

                await _logs.AddAsync(new flagging_config_log
                {
                    flag_config_id = existed.id,
                    action = "UPDATE",
                    old_data = oldJson,
                    new_data = System.Text.Json.JsonSerializer.Serialize(dto),
                    logged_at = now
                }, ct);
                await _logs.SaveAsync(ct);

                return _mapper.Map<FlaggingConfigDto>(existed);
            }
        }

        public async Task<FlaggingConfigDto?> GetByIdAsync(long id, CancellationToken ct)
            => _mapper.Map<FlaggingConfigDto?>(await _configs.GetAsync(id, ct));

        public async Task<FlaggingConfigDto?> GetByAnalyteAsync(string analyteCode, CancellationToken ct)
            => _mapper.Map<FlaggingConfigDto?>(await _configs.GetByAnalyteAsync(analyteCode, ct));

        public async Task<List<FlaggingConfigDto>> ListAsync(CancellationToken ct)
            => _mapper.Map<List<FlaggingConfigDto>>(await _configs.ListAsync(ct));

        public async Task SetActiveAsync(long id, bool active, string updatedBy, CancellationToken ct)
        {
            var e = await _configs.GetAsync(id, ct) ?? throw new KeyNotFoundException("Config not found");
            e.active = active; e.updated_by = updatedBy; e.updated_at = _clock.GetCurrentInstant().InUtc().LocalDateTime;
            await _configs.UpdateAsync(e, ct);
            await _configs.SaveAsync(ct);

            await _logs.AddAsync(new flagging_config_log
            {
                flag_config_id = e.id,
                action = active ? "ACTIVATE" : "DEACTIVATE",
                logged_at = e.updated_at
            }, ct);
            await _logs.SaveAsync(ct);
        }
    }
}
