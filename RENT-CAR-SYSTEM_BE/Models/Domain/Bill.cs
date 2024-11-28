using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Bill
{
    public string BillId { get; set; } = null!;

    public string? AgreementId { get; set; }

    public string? PaymentMethod { get; set; }

    public DateOnly Date { get; set; }

    public string? OrderDescription { get; set; }

    public string Status { get; set; } = null!;

    public virtual RentalAgreement? Agreement { get; set; }
}
