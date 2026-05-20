using System;
using System.Collections.Generic;

namespace RealEstateManagementAPI.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public int BuyerId { get; set; }

    public int AgentId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Agent { get; set; } = null!;

    public virtual User Buyer { get; set; } = null!;

    public virtual Property Property { get; set; } = null!;
}
