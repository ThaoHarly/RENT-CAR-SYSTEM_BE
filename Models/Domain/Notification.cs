using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Notification
{
    public string NotificationId { get; set; } = Guid.NewGuid().ToString();

    public string SenderId { get; set; } = null!;

    public string ReceiverId { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateOnly NotificationDate { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
