using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Car
{
    public string CarId { get; set; } = null!;

    public string VehicleId { get; set; } = null!;

    public string CarBrand { get; set; } = null!;

    public string FuelType { get; set; } = null!;

    public int SeatingCapacity { get; set; }

    public double? ChargingTime { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
