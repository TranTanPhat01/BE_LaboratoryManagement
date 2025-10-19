using AutoMapper;
using NodaTime;
using Testorder_service.Mapping.Converters;
using Testorder_service.Models.Dto;
using TestOrderService.Models;

namespace Testorder_service.Mapping.Profiles
{
    public class TestResultsProfile : Profile
    {
        public TestResultsProfile()
        {
            CreateMap<string?, LocalDateTime?>().ConvertUsing<StringToLocalDateTime>();
            CreateMap<LocalDateTime?, string?>().ConvertUsing<LocalDateTimeToString>();

            CreateMap<test_result, TestResultDto>();
            CreateMap<CreateTestResultDto, test_result>();
            CreateMap<UpdateTestResultDto, test_result>()
                .ForAllMembers(m => m.Condition((src, dest, val) => val != null));
        }
    }
}
