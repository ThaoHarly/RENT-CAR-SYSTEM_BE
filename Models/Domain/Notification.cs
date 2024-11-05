using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Notification
{
<<<<<<< HEAD
    public string NotificationId { get; set; } = Guid.NewGuid().ToString();

    public string SenderId { get; set; }

    public string ReceiverId { get; set; }
=======
    public Guid NotificationId { get; set; } = Guid.NewGuid();

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7

    public string Message { get; set; } = null!;

    public DateOnly NotificationDate { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
