using AutoMapper;
using NodaTime;
using Testorder_service.Mapping.Converters;
using Testorder_service.Models.Dto;
using TestOrderService.Models;

namespace Testorder_service.Mapping.Profiles
{
    public class ResultCommentsProfile : Profile
    {
        public ResultCommentsProfile()
        {
            CreateMap<string?, LocalDateTime?>().ConvertUsing<StringToLocalDateTime>();
            CreateMap<LocalDateTime?, string?>().ConvertUsing<LocalDateTimeToString>();

            CreateMap<result_comment, ResultCommentDto>();
            CreateMap<UpsertResultCommentDto, result_comment>()
                .ForAllMembers(m => m.Condition((src, dest, val) => val != null));
        }
    }
}
