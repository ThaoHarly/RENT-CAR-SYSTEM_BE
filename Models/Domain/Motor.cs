using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Motor
{
    public Guid MotorId { get; set; } 

    public Guid VehicleId { get; set; } 

    public string MotorImage { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
