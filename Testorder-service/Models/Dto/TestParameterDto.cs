namespace Testorder_service.Models.Dto
{
    public class TestParameterDto
    {
        public long Id { get; set; }
        public long TestResultId { get; set; }
        public string ParamName { get; set; } = default!;
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public decimal? NormalRangeMin { get; set; }
        public decimal? NormalRangeMax { get; set; }
        public string? NormalUnit { get; set; }
        public bool? GenderSpecific { get; set; }
        public decimal? MaleRangeMin { get; set; }
        public decimal? MaleRangeMax { get; set; }
        public decimal? FemaleRangeMin { get; set; }
        public decimal? FemaleRangeMax { get; set; }
        public string? Status { get; set; }
        public string? Flag { get; set; }
        public long? FlagConfigId { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public bool? DeletedFlag { get; set; }
        public string? ReagentRefId { get; set; }
        public string? ReagentCode { get; set; }
        public string? ReagentLot { get; set; }
    }
    // CREATE
    public class CreateTestParameterDto
    {
        public long TestResultId { get; set; }
        public string ParamName { get; set; } = default!;
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public decimal? NormalRangeMin { get; set; }
        public decimal? NormalRangeMax { get; set; }
        public string? NormalUnit { get; set; }
        public bool? GenderSpecific { get; set; }
        public decimal? MaleRangeMin { get; set; }
        public decimal? MaleRangeMax { get; set; }
        public decimal? FemaleRangeMin { get; set; }
        public decimal? FemaleRangeMax { get; set; }
        public string? Status { get; set; }
        public string? Flag { get; set; }
        public long? FlagConfigId { get; set; }
        public string? ReagentRefId { get; set; }
        public string? ReagentCode { get; set; }
        public string? ReagentLot { get; set; }
    }

    // UPDATE (partial)
    public class UpdateTestParameterDto
    {
        public long Id { get; set; }
        public decimal? NormalRangeMin { get; set; }
        public decimal? NormalRangeMax { get; set; }
        public string? NormalUnit { get; set; }
        public bool? GenderSpecific { get; set; }
        public decimal? MaleRangeMin { get; set; }
        public decimal? MaleRangeMax { get; set; }
        public decimal? FemaleRangeMin { get; set; }
        public decimal? FemaleRangeMax { get; set; }
        public string? Status { get; set; }
        public string? Flag { get; set; }
        public long? FlagConfigId { get; set; }
        public string? ReagentRefId { get; set; }
        public string? ReagentCode { get; set; }
        public string? ReagentLot { get; set; }
    }
}
