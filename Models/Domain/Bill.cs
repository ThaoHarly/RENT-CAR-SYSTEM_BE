﻿using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Bill
{
<<<<<<< HEAD
    public string BillId { get; set; } = Guid.NewGuid().ToString();

    public string CusId { get; set; } = null!;

    public string AgreementId { get; set; }
=======
    public Guid BillId { get; set; }

    public string CusId { get; set; } = null!;

    public Guid AgreementId { get; set; }
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public DateOnly Date { get; set; }

    public string PaymentImg { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual RentalAgreement Agreement { get; set; }

    public virtual Customer Cus { get; set; } 
}
