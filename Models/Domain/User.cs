using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class User
{
<<<<<<< HEAD
    public string UserId { get; set; } = Guid.NewGuid().ToString();
=======
    public Guid UserId { get; set; } = Guid.NewGuid();
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Nationality { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Notification> NotificationReceivers { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationSenders { get; set; } = new List<Notification>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual VehicleHireService? VehicleHireService { get; set; }
}
