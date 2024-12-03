using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Customer
{
    public string LicenseId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Class { get; set; } = null!;

    public DateOnly Expire { get; set; }

    public string Image { get; set; } = null!;

    public virtual ICollection<RentalAgreement> RentalAgreements { get; set; } = new List<RentalAgreement>();

    public virtual User User { get; set; } = null!;
}
