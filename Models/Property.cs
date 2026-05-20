using System;
using System.Collections.Generic;

namespace RealEstateManagementAPI.Models;

public partial class Property
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string Location { get; set; } = null!;

    public string PropertyType { get; set; } = null!;

    public string? Status { get; set; }

    public int AgentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Agent { get; set; } = null!;

    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Viewing> Viewings { get; set; } = new List<Viewing>();
}
