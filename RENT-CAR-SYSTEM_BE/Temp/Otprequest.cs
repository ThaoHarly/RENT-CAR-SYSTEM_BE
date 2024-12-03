using System;
using System.Collections.Generic;

namespace RentCarSystem.Temp;

public partial class Otprequest
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? Otp { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public virtual User? User { get; set; }
}
