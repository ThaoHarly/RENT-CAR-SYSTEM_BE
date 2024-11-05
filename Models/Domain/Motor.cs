using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Motor
{
<<<<<<< HEAD
    public string MotorId { get; set; } 

    public string VehicleId { get; set; } 
=======
    public Guid MotorId { get; set; } 

    public Guid VehicleId { get; set; } 
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public string MotorImage { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
