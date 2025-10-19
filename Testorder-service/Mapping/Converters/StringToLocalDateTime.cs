using AutoMapper;
using NodaTime;
using NodaTime.Text;

namespace Testorder_service.Mapping.Converters
{
    public sealed class StringToLocalDateTime : ITypeConverter<string?, LocalDateTime?>
    {
        private static readonly LocalDateTimePattern P = LocalDateTimePattern.ExtendedIso;
        public LocalDateTime? Convert(string? src, LocalDateTime? dest, ResolutionContext ctx)
            => string.IsNullOrWhiteSpace(src) ? null : P.Parse(src).GetValueOrThrow();
    }

    // LocalDateTime? -> string (ISO)
    public sealed class LocalDateTimeToString : ITypeConverter<LocalDateTime?, string?>
    {
        private static readonly LocalDateTimePattern P = LocalDateTimePattern.ExtendedIso;
        public string? Convert(LocalDateTime? src, string? dest, ResolutionContext ctx)
            => src.HasValue ? P.Format(src.Value) : null;
    }
}
