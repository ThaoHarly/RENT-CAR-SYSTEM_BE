using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Individual
{
    public string IdvId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual VehicleHireService User { get; set; } = null!;
}
