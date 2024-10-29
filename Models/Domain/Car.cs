using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Car
{
    public Guid CarId { get; set; }

    public Guid VehicleId { get; set; }

    public string CarBrand { get; set; } = null!;

    public string FuelType { get; set; } = null!;

    public int SeatingCapacity { get; set; }

    public string CarImage { get; set; } = null!;

    public double? ChargingTime { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
