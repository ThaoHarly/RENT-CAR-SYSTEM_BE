using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Individual
{
    public string IdvId { get; set; } = Guid.NewGuid().ToString();

    public string UserId { get; set; }

    public virtual VehicleHireService User { get; set; } = null!;
}
