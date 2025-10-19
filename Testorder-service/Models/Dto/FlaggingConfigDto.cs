namespace Testorder_service.Models.Dto
{
    public class FlaggingConfigDto
    {
        public long Id { get; set; }
        public string AnalyteCode { get; set; } = default!;
        public string? AnalyteName { get; set; }
        public decimal? NormalMin { get; set; }
        public decimal? NormalMax { get; set; }
        public decimal? CriticalMin { get; set; }
        public decimal? CriticalMax { get; set; }
        public string? Unit { get; set; }
        public string? FlagType { get; set; }
        public string? Version { get; set; }
        public string? UpdatedBy { get; set; }
        public string UpdatedAt { get; set; } = default!; // ISO
        public bool? Active { get; set; }
    }
    public class UpsertFlaggingConfigDto
    {
        public string AnalyteCode { get; set; } = default!;
        public string? AnalyteName { get; set; }
        public decimal? NormalMin { get; set; }
        public decimal? NormalMax { get; set; }
        public decimal? CriticalMin { get; set; }
        public decimal? CriticalMax { get; set; }
        public string? Unit { get; set; }
        public string? FlagType { get; set; }   // ví dụ: "LOW/HIGH/NORMAL" rule-set
        public string? Version { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? Active { get; set; } = true;
    }
}
