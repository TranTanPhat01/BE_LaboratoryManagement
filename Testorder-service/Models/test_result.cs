using System;
using System.Collections.Generic;
using NodaTime;

namespace TestOrderService.Models;

public partial class test_result
{
    public long id { get; set; }

    public long sample_id { get; set; }

    public string test_code { get; set; } = null!;

    public string test_name { get; set; } = null!;

    public string? result_value { get; set; }

    public string? unit { get; set; }

    public string? reference_range { get; set; }

    public string? flag { get; set; }

    public long? flag_config_id { get; set; }

    public string? ai_reviewed_by { get; set; }

    public LocalDateTime? ai_reviewed_at { get; set; }

    public string? reviewed_by { get; set; }

    public LocalDateTime? reviewed_at { get; set; }

    public string? comments { get; set; }

    public string? status { get; set; }

    public LocalDateTime created_at { get; set; }

    public LocalDateTime? updated_at { get; set; }

    public bool? deleted_flag { get; set; }

    public string? reagent_ref_id { get; set; }

    public string? reagent_code { get; set; }

    public string? reagent_lot { get; set; }

    public virtual flagging_configuration? flag_config { get; set; }

    public virtual ICollection<result_comment> result_comments { get; set; } = new List<result_comment>();

    public virtual blood_sample sample { get; set; } = null!;

    public virtual ICollection<test_parameter> test_parameters { get; set; } = new List<test_parameter>();
}
