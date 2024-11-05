using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class VehicleHireService
{
<<<<<<< HEAD
    public string UserId { get; set; } 
=======
    public Guid UserId { get; set; } 
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public string ServiceType { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string BankAccount { get; set; } = null!;

    public virtual Business? Business { get; set; }

    public virtual Individual? Individual { get; set; }

    public virtual ICollection<RentalAgreement> RentalAgreements { get; set; } = new List<RentalAgreement>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
