using System;
using System.Collections.Generic;

namespace RealEstateManagementAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string PasswordHash { get; set; } = null!;
    public bool AgentRequestPending { get; set; }

    public Role Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();

    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();

    public virtual ICollection<Transaction> TransactionAgents { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionBuyers { get; set; } = new List<Transaction>();

    public virtual ICollection<Viewing> Viewings { get; set; } = new List<Viewing>();
}
