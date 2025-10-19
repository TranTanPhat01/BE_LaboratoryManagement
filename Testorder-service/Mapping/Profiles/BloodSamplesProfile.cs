using AutoMapper;
using NodaTime;
using Testorder_service.Mapping.Converters;
using Testorder_service.Models.Dto;
using TestOrderService.Models;

namespace Testorder_service.Mapping.Profiles
{
    public class BloodSamplesProfile : Profile
    {
        public BloodSamplesProfile()
        {
            // Converters NodaTime
            CreateMap<string?, LocalDateTime?>().ConvertUsing<StringToLocalDateTime>();
            CreateMap<LocalDateTime?, string?>().ConvertUsing<LocalDateTimeToString>();

            // Entity -> DTO
            CreateMap<blood_sample, BloodSampleDto>();

            // DTO -> Entity (create)
            CreateMap<CreateBloodSampleDto, blood_sample>();

            // DTO -> Entity (patch)
            CreateMap<UpdateBloodSampleDto, blood_sample>()
                .ForAllMembers(m => m.Condition((src, dest, val) => val != null));
        }

    }
}
