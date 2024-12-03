using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Bill
{
    public string BillId { get; set; } = null!;

    public string? PaymentMethod { get; set; }

    public DateOnly Date { get; set; }

    public string? OrderDescription { get; set; }

    public string Status { get; set; } = null!;

    public string? AgreementId { get; set; }

    public virtual RentalAgreement? Agreement { get; set; }
}
