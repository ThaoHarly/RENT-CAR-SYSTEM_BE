using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Nationality { get; set; }

    public bool? IsVerified { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Notification> NotificationReceivers { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationSenders { get; set; } = new List<Notification>();

    public virtual ICollection<Otprequest> Otprequests { get; set; } = new List<Otprequest>();

    public virtual ICollection<PasswordResetRequest> PasswordResetRequests { get; set; } = new List<PasswordResetRequest>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual VehicleHireService? VehicleHireService { get; set; }
}
