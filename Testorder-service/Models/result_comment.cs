using System;
using System.Collections.Generic;
using NodaTime;

namespace TestOrderService.Models;

public partial class result_comment
{
    public long id { get; set; }

    public long result_id { get; set; }

    public long? sample_id { get; set; }

    public string commented_by { get; set; } = null!;

    public LocalDateTime commented_at { get; set; }

    public string? comment_text { get; set; }

    public bool? is_edited { get; set; }

    public LocalDateTime? edited_at { get; set; }

    public bool? deleted_flag { get; set; }

    public virtual test_result result { get; set; } = null!;

    public virtual blood_sample? sample { get; set; }
}
