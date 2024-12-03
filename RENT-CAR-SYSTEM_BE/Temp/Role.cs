using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Role
{
    public string RoleId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
