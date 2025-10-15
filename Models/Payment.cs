// =============================================================================
// FILE: Payment.cs
// PROJECT: SavingsClub
// =============================================================================
// Represents a financial transaction (income or expense) associated with
// a specific member in the SavingsClub application.
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsClub.Models;

// =============================================================================
// CLASS: Payment
// =============================================================================
// Defines the data structure for payments, including amount, type,
// description, date, and the member relationship.
// =============================================================================
public class Payment
{
    // -------------------------------------------------------------------------
    // PRIMARY KEY
    // -------------------------------------------------------------------------
    public int Id { get; set; }

    // -------------------------------------------------------------------------
    // PROPERTIES
    // -------------------------------------------------------------------------

    [Display(Name = "Income?")]
    public bool IsIncome { get; set; }

    [Range(0.01, 1_000_000), Display(Name = "Amount")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required, MaxLength(50), Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [DataType(DataType.Date), Display(Name = "Date"), NotInFuture]
    public DateTime Date { get; set; } = DateTime.Today;

    // -------------------------------------------------------------------------
    // RELATIONSHIPS
    // -------------------------------------------------------------------------
    // Each payment belongs to exactly one member.
    // -------------------------------------------------------------------------
    [Display(Name = "Member")]
    public int MemberId { get; set; }
    public Member? Member { get; set; }
}
