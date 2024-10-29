using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class RentalAgreement
{
    public Guid AgreementId { get; set; } 

    public Guid VehicleId { get; set; }

    public string CusId { get; set; } = null!;

    public Guid ServiceId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Status { get; set; }

    public double? DepositAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Customer? Cus { get; set; }

    public virtual VehicleHireService? Service { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
