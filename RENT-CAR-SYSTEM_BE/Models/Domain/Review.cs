using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Review
{

    public string ReviewId { get; set; } 

    public string CusId { get; set; } = null!;

    public string VehicleId { get; set; } 

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateOnly ReviewDate { get; set; }

    public virtual Customer Cus { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
