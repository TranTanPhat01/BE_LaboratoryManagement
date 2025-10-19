using AutoMapper;
using NodaTime;
using Testorder_service.Mapping.Converters;
using Testorder_service.Models.Dto;
using TestOrderService.Models;

namespace Testorder_service.Mapping.Profiles
{
    public class FlaggingConfigsProfile : Profile
    {
        public FlaggingConfigsProfile()
        {
            CreateMap<string?, LocalDateTime?>().ConvertUsing<StringToLocalDateTime>();
            CreateMap<LocalDateTime?, string?>().ConvertUsing<LocalDateTimeToString>();

            CreateMap<flagging_configuration, FlaggingConfigDto>();
            CreateMap<UpsertFlaggingConfigDto, flagging_configuration>()
                .ForAllMembers(m => m.Condition((src, dest, val) => val != null));
        }
    }
}
