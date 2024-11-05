using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Role
{
    public string RoleId { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } 

    public string Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
