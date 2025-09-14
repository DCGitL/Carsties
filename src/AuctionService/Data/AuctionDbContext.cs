using System;
using AuctionService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
    }

    protected AuctionDbContext()
    {
    }

    public DbSet<Auction> Auctions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One-to-one Auction ↔ Item
        modelBuilder.Entity<Auction>()
            .HasOne(a => a.Item)
            .WithOne(i => i.Auction)
            .HasForeignKey<Item>(i => i.AuctionId)
            .OnDelete(DeleteBehavior.Cascade); // ✅ ensures deletion of Item

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();



    }
}
