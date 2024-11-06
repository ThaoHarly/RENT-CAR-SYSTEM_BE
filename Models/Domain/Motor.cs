using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Motor
{
    public string MotorId { get; set; } 

    public string VehicleId { get; set; } 

    public string MotorImage { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
