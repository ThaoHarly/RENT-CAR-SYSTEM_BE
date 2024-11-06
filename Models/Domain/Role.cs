using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Role
{
<<<<<<< HEAD
    public string RoleId { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } 
=======
    public Guid RoleId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } 
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public string Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
