using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace RealEstateManagementAPI.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Inquiry> Inquiries { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<Viewing> Viewings { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
   
 protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<User>()
        .Property(u => u.Role)
        .HasConversion<string>();

        modelBuilder.Entity<Property>()
            .HasOne(p => p.Agent)
            .WithMany(u => u.Properties)
            .HasForeignKey(p => p.AgentId);

        modelBuilder.Entity<Inquiry>()
            .HasOne(i => i.User)
            .WithMany(u => u.Inquiries)
            .HasForeignKey(i => i.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Property)
            .WithMany(p => p.Transactions)
            .HasForeignKey(t => t.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Transaction>()
        .HasOne(t => t.Agent)
        .WithMany(u => u.TransactionAgents)
        .HasForeignKey(t => t.AgentId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Transaction>()
        .HasOne(t => t.Property)
        .WithMany()
        .HasForeignKey(t => t.PropertyId);
    }
}



