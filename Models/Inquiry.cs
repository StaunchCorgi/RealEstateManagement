using System;
using System.Collections.Generic;

namespace RealEstateManagementAPI.Models;

public partial class Inquiry
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PropertyId { get; set; }

    public string? Message { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Property Property { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
