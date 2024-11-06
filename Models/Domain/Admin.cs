using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Admin
{
<<<<<<< HEAD
    public string AdminId { get; set; }
=======
    public Guid AdminId { get; set; }
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public DateTime LastLogin { get; set; }

    public virtual User AdminNavigation { get; set; } = null!;

    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = new List<ApprovalRequest>();
}
