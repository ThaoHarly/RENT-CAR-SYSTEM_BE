using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Notification
{
    public Guid NotificationId { get; set; } = Guid.NewGuid();

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public string Message { get; set; } = null!;

    public DateOnly NotificationDate { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
