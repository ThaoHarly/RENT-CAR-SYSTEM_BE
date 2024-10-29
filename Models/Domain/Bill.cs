using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Bill
{
    public Guid BillId { get; set; }

    public string CusId { get; set; } = null!;

    public Guid AgreementId { get; set; }

    public DateOnly Date { get; set; }

    public string PaymentImg { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual RentalAgreement Agreement { get; set; }

    public virtual Customer Cus { get; set; } 
}
