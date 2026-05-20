using System;
using System.Collections.Generic;

namespace RealEstateManagementAPI.Models;

public partial class Viewing
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public int UserId { get; set; }

    public DateTime ScheduledDate { get; set; }

    public string? Status { get; set; }

    public virtual Property Property { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
