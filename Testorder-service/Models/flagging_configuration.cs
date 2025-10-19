using System;
using System.Collections.Generic;
using NodaTime;

namespace TestOrderService.Models;

public partial class flagging_configuration
{
    internal LocalDateTime created_at;

    public long id { get; set; }

    public string analyte_code { get; set; } = null!;

    public string? analyte_name { get; set; }

    public decimal? normal_min { get; set; }

    public decimal? normal_max { get; set; }

    public decimal? critical_min { get; set; }

    public decimal? critical_max { get; set; }

    public string? unit { get; set; }

    public string? flag_type { get; set; }

    public string? version { get; set; }

    public string? updated_by { get; set; }

    public LocalDateTime updated_at { get; set; }

    public bool? active { get; set; }

    public virtual ICollection<flagging_config_log> flagging_config_logs { get; set; } = new List<flagging_config_log>();

    public virtual ICollection<test_parameter> test_parameters { get; set; } = new List<test_parameter>();

    public virtual ICollection<test_result> test_results { get; set; } = new List<test_result>();
}
