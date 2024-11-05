using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Individual
{
    public Guid IdvId { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public virtual VehicleHireService User { get; set; } = null!;
}
