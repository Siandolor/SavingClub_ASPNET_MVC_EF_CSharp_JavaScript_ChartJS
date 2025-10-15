// =============================================================================
// FILE: Member.cs
// PROJECT: SavingsClub
// =============================================================================
// Represents a member entity within the SavingsClub application.
// Each member can be linked to multiple payments.
// =============================================================================

using System.ComponentModel.DataAnnotations;

namespace SavingsClub.Models;

// =============================================================================
// CLASS: Member
// =============================================================================
// Defines the core data structure for members, including basic identification,
// status, and relationship mapping to payments.
// =============================================================================
public class Member
{
    // -------------------------------------------------------------------------
    // PRIMARY KEY
    // -------------------------------------------------------------------------
    public int Id { get; set; }

    // -------------------------------------------------------------------------
    // PROPERTIES
    // -------------------------------------------------------------------------

    [Required, MaxLength(50), Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50), Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Image (optional)")]
    public string? ImagePath { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    // -------------------------------------------------------------------------
    // RELATIONSHIPS
    // -------------------------------------------------------------------------
    // One Member → Many Payments
    // -------------------------------------------------------------------------
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();


    // -------------------------------------------------------------------------
    // COMPUTED PROPERTIES
    // -------------------------------------------------------------------------
    // Full name is automatically combined from first and last name.
    // -------------------------------------------------------------------------
    public string FullName => $"{FirstName} {LastName}";
}
