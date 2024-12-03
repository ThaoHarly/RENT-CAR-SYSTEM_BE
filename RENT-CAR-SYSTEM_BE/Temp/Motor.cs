using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Motor
{
    public string MotorId { get; set; } = null!;

    public string VehicleId { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
