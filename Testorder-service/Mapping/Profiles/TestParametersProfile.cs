using AutoMapper;
using NodaTime;
using Testorder_service.Mapping.Converters;
using Testorder_service.Models.Dto;
using TestOrderService.Models;

namespace Testorder_service.Mapping.Profiles
{
    public class TestParametersProfile : Profile
    {
        public TestParametersProfile()
        {
            CreateMap<string?, LocalDateTime?>().ConvertUsing<StringToLocalDateTime>();
            CreateMap<LocalDateTime?, string?>().ConvertUsing<LocalDateTimeToString>();

            CreateMap<test_parameter, TestParameterDto>();
            CreateMap<CreateTestParameterDto, test_parameter>();
            CreateMap<UpdateTestParameterDto, test_parameter>()
                .ForAllMembers(m => m.Condition((src, dest, val) => val != null));
        }
    }
}
