using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class VehicleHireService
{
    public Guid UserId { get; set; } 

    public string ServiceType { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string BankAccount { get; set; } = null!;

    public virtual Business? Business { get; set; }

    public virtual Individual? Individual { get; set; }

    public virtual ICollection<RentalAgreement> RentalAgreements { get; set; } = new List<RentalAgreement>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
