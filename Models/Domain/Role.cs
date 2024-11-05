using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Role
{
    public Guid RoleId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } 

    public string Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
