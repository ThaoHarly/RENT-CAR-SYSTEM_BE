using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class ApprovalRequest
{

    public string RequestId { get; set; } = Guid.NewGuid().ToString();

    public string AdminId { get; set; }

    public string BsnId { get; set; }


    public DateOnly? RequestDay { get; set; }

    public string? Status { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Business Bsn { get; set; } = null!;
}
