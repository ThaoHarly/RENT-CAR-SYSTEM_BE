using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class PasswordResetRequest
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsUsed { get; set; }

    public virtual User? User { get; set; }
}
