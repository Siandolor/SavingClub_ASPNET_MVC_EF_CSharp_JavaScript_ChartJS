// =============================================================================
// FILE: AppDbContext.cs
// PROJECT: SavingsClub
// =============================================================================
// Defines the Entity Framework Core database context used throughout the
// SavingsClub application. Configures entities, relationships, and constraints
// for Members and Payments.
// =============================================================================

using Microsoft.EntityFrameworkCore;
using SavingsClub.Models;

namespace SavingsClub.Data;

// =============================================================================
// CLASS: AppDbContext
// =============================================================================
// Provides access to the underlying database and defines entity relationships
// between Members and Payments.
// =============================================================================
public class AppDbContext : DbContext
{
    // -------------------------------------------------------------------------
    // CONSTRUCTOR
    // -------------------------------------------------------------------------
    // Injects the database context configuration defined in Program.cs.
    // -------------------------------------------------------------------------
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // -------------------------------------------------------------------------
    // DB SETS
    // -------------------------------------------------------------------------
    // Define the database tables for EF Core.
    // -------------------------------------------------------------------------
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Payment> Payments => Set<Payment>();


    // -------------------------------------------------------------------------
    // MODEL CONFIGURATION
    // -------------------------------------------------------------------------
    // Configures entity schemas, field constraints, relationships, and indices.
    // -------------------------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== MEMBER CONFIGURATION =========================================
        modelBuilder.Entity<Member>(e =>
        {
            e.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            e.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();
        });

        // ===== PAYMENT CONFIGURATION ========================================
        modelBuilder.Entity<Payment>(e =>
        {
            e.Property(p => p.Description)
                .HasMaxLength(50)
                .IsRequired();

            e.Property(p => p.Amount)
                .HasPrecision(18, 2);

            // Relationship: one member → many payments
            e.HasOne(p => p.Member)
                .WithMany(m => m.Payments)
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for faster filtering by date
            e.HasIndex(p => p.Date);
        });
    }
}
