using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Customer
{
    public string LicenseId { get; set; } = null!;

<<<<<<< HEAD
    public string UserId { get; set; }
=======
    public Guid UserId { get; set; }
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public string Class { get; set; } = null!;

    public DateOnly Expire { get; set; }

    public string Image { get; set; } = null!;

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<RentalAgreement> RentalAgreements { get; set; } = new List<RentalAgreement>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;
}
