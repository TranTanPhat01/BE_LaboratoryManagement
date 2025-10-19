using System;
using System.Collections.Generic;
using NodaTime;

namespace TestOrderService.Models;

public partial class test_parameter
{
    public long id { get; set; }

    public long test_result_id { get; set; }

    public string param_name { get; set; } = null!;

    public string? abbreviation { get; set; }

    public string? description { get; set; }

    public decimal? normal_range_min { get; set; }

    public decimal? normal_range_max { get; set; }

    public string? normal_unit { get; set; }

    public bool? gender_specific { get; set; }

    public decimal? male_range_min { get; set; }

    public decimal? male_range_max { get; set; }

    public decimal? female_range_min { get; set; }

    public decimal? female_range_max { get; set; }

    public string? status { get; set; }

    public string? flag { get; set; }

    public long? flag_config_id { get; set; }

    public LocalDateTime created_at { get; set; }

    public LocalDateTime? updated_at { get; set; }

    public bool? deleted_flag { get; set; }

    public string? reagent_ref_id { get; set; }

    public string? reagent_code { get; set; }

    public string? reagent_lot { get; set; }

    public virtual flagging_configuration? flag_config { get; set; }

    public virtual test_result test_result { get; set; } = null!;
}
