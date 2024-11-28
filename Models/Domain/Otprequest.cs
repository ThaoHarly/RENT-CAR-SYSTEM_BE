using System;
using System.Collections.Generic;

namespace RentCarSystem.Models.Domain;

public partial class Otprequest
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? OTP { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public virtual User? User { get; set; }
}
