using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class PasswordResetRequest
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string ResetToken { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsUsed { get; set; }

    public virtual User User { get; set; } = null!;
}
