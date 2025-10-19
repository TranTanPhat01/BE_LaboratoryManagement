namespace Testorder_service.Models.Dto
{
    public class TestResultDto
    {
        public long Id { get; set; }
        public long SampleId { get; set; }
        public string TestCode { get; set; } = default!;
        public string TestName { get; set; } = default!;
        public string? ResultValue { get; set; }
        public string? Unit { get; set; }
        public string? ReferenceRange { get; set; }
        public string? Flag { get; set; }
        public long? FlagConfigId { get; set; }
        public string? AiReviewedBy { get; set; }
        public string? AiReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }
        public string? ReviewedAt { get; set; }
        public string? Comments { get; set; }
        public string? Status { get; set; }
        public string CreatedAt { get; set; } = default!;
        public string? UpdatedAt { get; set; }
        public bool? DeletedFlag { get; set; }
        public string? ReagentRefId { get; set; }
        public string? ReagentCode { get; set; }
        public string? ReagentLot { get; set; }
    }
    public class CreateTestResultDto
    {
        public long SampleId { get; set; }
        public string TestCode { get; set; } = default!;
        public string TestName { get; set; } = default!;
        public string? ResultValue { get; set; }
        public string? Unit { get; set; }
        public string? ReferenceRange { get; set; }
    }

    // UPDATE (partial)
    public class UpdateTestResultDto
    {
        public long Id { get; set; }
        public string? ResultValue { get; set; }
        public string? Unit { get; set; }
        public string? ReferenceRange { get; set; }
        public string? Flag { get; set; }
        public long? FlagConfigId { get; set; }
        public string? AiReviewedBy { get; set; }
        public string? AiReviewedAt { get; set; }   // ISO
        public string? ReviewedBy { get; set; }
        public string? ReviewedAt { get; set; }     // ISO
        public string? Comments { get; set; }
        public string? Status { get; set; }
    }

}
