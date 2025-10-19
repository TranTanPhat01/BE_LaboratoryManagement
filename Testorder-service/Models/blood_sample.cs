using System;
using System.Collections.Generic;
using NodaTime;

namespace TestOrderService.Models;

public partial class blood_sample
{
    public long id { get; set; }

    public string sample_code { get; set; } = null!;

    public string? barcode { get; set; }

    public long? patient_id { get; set; }

    public long? medical_record_id { get; set; }

    public string? status { get; set; }

    public LocalDateTime? collected_at { get; set; }

    public LocalDateTime? analyzed_at { get; set; }

    public bool? result_published { get; set; }

    public string? error_message { get; set; }

    public LocalDateTime created_at { get; set; }

    public LocalDateTime? updated_at { get; set; }

    public virtual ICollection<result_comment> result_comments { get; set; } = new List<result_comment>();

    public virtual ICollection<test_result> test_results { get; set; } = new List<test_result>();
}
