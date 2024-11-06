using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class ApprovalRequest
{
<<<<<<< HEAD
    public string RequestId { get; set; } = Guid.NewGuid().ToString();

    public string AdminId { get; set; }

    public string BsnId { get; set; }
=======
    public Guid RequestId { get; set; } = Guid.NewGuid();

    public Guid AdminId { get; set; }

    public Guid BsnId { get; set; }
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public DateOnly? RequestDay { get; set; }

    public string? Status { get; set; }

    public virtual Admin Admin { get; set; } = null!;

    public virtual Business Bsn { get; set; } = null!;
}
