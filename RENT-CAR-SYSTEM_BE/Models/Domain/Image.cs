using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Image
{
    public string ImageId { get; set; } = Guid.NewGuid().ToString();

    public string? VehicleId { get; set; }

    public string? ImagePath { get; set; }

    public DateTime? Upload { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
