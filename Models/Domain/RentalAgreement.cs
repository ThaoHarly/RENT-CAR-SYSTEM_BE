using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class RentalAgreement
{
    public string AgreementId { get; set; } = Guid.NewGuid().ToString();

    public string? VehicleId { get; set; }

    public string? CusId { get; set; }

    public string? ServiceId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Status { get; set; }

    public double? DepositAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? BillId { get; set; }

    public virtual Bill? Bill { get; set; }

    public virtual Customer? Cus { get; set; }

    public virtual VehicleHireService? Service { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
