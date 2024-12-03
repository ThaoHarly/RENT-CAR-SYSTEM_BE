using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class ApprovalRequest
{
    public string RequestId { get; set; } = null!;

    public string AdminId { get; set; } = null!;

    public string BsnId { get; set; } = null!;

    public DateOnly? RequestDay { get; set; }

    public string? Status { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Business Bsn { get; set; } = null!;
}
