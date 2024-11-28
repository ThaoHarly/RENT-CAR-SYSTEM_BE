using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Bill
{
    public string BillId { get; set; } = Guid.NewGuid().ToString();

    public string? PaymentMethod { get; set; }

    public DateOnly Date { get; set; }

    public string? OrderDescription { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<RentalAgreement> RentalAgreements { get; set; } = new List<RentalAgreement>();
}
