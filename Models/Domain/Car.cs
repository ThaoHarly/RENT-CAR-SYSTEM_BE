using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Car
{
<<<<<<< HEAD
    public string CarId { get; set; }

    public string VehicleId { get; set; }
=======
    public Guid CarId { get; set; }

    public Guid VehicleId { get; set; }
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public string CarBrand { get; set; } = null!;

    public string FuelType { get; set; } = null!;

    public int SeatingCapacity { get; set; }

    public string CarImage { get; set; } = null!;

    public double? ChargingTime { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
