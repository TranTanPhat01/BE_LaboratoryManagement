namespace Testorder_service.Models.Dto
{
    public class FlaggingLogDto
    {
        public long Id { get; set; }
        public long FlagConfigId { get; set; }
        public string Action { get; set; } = default!;
        public string? OldData { get; set; }
        public string? NewData { get; set; }
        public string? Source { get; set; }
        public string LoggedAt { get; set; } = default!;
    }
}
