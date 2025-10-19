using System;
using System.Collections.Generic;
using NodaTime;

namespace TestOrderService.Models;

public partial class flagging_config_log
{
    public long id { get; set; }

    public long flag_config_id { get; set; }

    public string action { get; set; } = null!;

    public string? old_data { get; set; }

    public string? new_data { get; set; }

    public string? source { get; set; }

    public LocalDateTime logged_at { get; set; }

    public virtual flagging_configuration flag_config { get; set; } = null!;
}
