namespace Testorder_service.Models.Dto
{
    public class BloodSampleDto
    {
        public long Id { get; set; }
        public string SampleCode { get; set; } = default!;
        public string? Barcode { get; set; }
        public long? PatientId { get; set; }
        public long? MedicalRecordId { get; set; }
        public string? Status { get; set; }
        public string? CollectedAt { get; set; }
        public string? AnalyzedAt { get; set; }
        public bool? ResultPublished { get; set; }
        public string? ErrorMessage { get; set; }
        public string CreatedAt { get; set; } = default!;
        public string? UpdatedAt { get; set; }
    }

    public class CreateBloodSampleDto
    {
        public string SampleCode { get; set; } = default!;
        public string? Barcode { get; set; }
        public long? PatientId { get; set; }
        public long? MedicalRecordId { get; set; }
        public string? Status { get; set; }           // "NEW" / "PENDING" / ...
        public string? CollectedAt { get; set; }      // ISO "yyyy-MM-ddTHH:mm:ss"
        public string? AnalyzedAt { get; set; }       // ISO
        public bool? ResultPublished { get; set; }
        public string? ErrorMessage { get; set; }
    }

    // UPDATE (partial)
    public class UpdateBloodSampleDto
    {
        public long Id { get; set; }
        public string? Status { get; set; }
        public string? CollectedAt { get; set; }
        public string? AnalyzedAt { get; set; }
        public bool? ResultPublished { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
