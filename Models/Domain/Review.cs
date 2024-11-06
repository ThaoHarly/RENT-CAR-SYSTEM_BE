using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Review
{
<<<<<<< HEAD
    public string ReviewId { get; set; } 

    public string CusId { get; set; } = null!;

    public string VehicleId { get; set; } 
=======
    public Guid ReviewId { get; set; } 

    public string CusId { get; set; } = null!;

    public Guid VehicleId { get; set; } 
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateOnly ReviewDate { get; set; }

    public virtual Customer Cus { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
