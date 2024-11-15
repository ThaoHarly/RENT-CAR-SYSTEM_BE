using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Business
{
    public string BsnId { get; set; } = Guid.NewGuid().ToString();

    public string UserId { get; set; }

    public string Description { get; set; } = null!;

    public string BusinessImg { get; set; } = null!;

    public DateOnly RegistrationDate { get; set; }

    public double Vat { get; set; }

    public string IssuingLocation { get; set; } = null!;

    public DateOnly DateOfIssue { get; set; }

    public virtual ApprovalRequest? ApprovalRequest { get; set; }

    public virtual VehicleHireService User { get; set; } = null!;
}
