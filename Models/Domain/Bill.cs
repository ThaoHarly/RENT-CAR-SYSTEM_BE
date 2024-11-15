using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Bill
{
    public string BillId { get; set; } = Guid.NewGuid().ToString();

    public string CusId { get; set; } = null!;

    public string AgreementId { get; set; }

    public DateOnly Date { get; set; }

    public string PaymentImg { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual RentalAgreement Agreement { get; set; }

    public virtual Customer Cus { get; set; } 
}
