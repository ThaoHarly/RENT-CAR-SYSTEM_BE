using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Admin
{
    public string AdminId { get; set; } = null!;

    public DateTime LastLogin { get; set; }

    public virtual User AdminNavigation { get; set; } = null!;

    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = new List<ApprovalRequest>();
}
