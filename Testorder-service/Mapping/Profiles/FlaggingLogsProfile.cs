using AutoMapper;
using NodaTime;
using Testorder_service.Mapping.Converters;
using Testorder_service.Models.Dto;
using TestOrderService.Models;

namespace Testorder_service.Mapping.Profiles
{
    public class FlaggingLogsProfile : Profile
    {
        public FlaggingLogsProfile()
        {
            CreateMap<string?, LocalDateTime?>().ConvertUsing<StringToLocalDateTime>();
            CreateMap<LocalDateTime?, string?>().ConvertUsing<LocalDateTimeToString>();

            CreateMap<flagging_config_log, FlaggingLogDto>();
            
        }
    }
}
