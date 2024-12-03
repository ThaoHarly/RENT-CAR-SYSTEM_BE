using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Image
{
    public string ImageId { get; set; } = null!;

    public string? VehicleId { get; set; }

    public string? ImagePath { get; set; }

    public DateTime? Upload { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
