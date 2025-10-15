// =============================================================================
// FILE: PaymentFilterViewModel.cs
// PROJECT: SavingsClub
// =============================================================================
// Represents the filtering options and results used in payment list views.
// Allows filtering by text query, member, income/expense type, and record limit.
// =============================================================================

using Microsoft.AspNetCore.Mvc.Rendering;
using SavingsClub.Models;

namespace SavingsClub.ViewModels;

// =============================================================================
// CLASS: PaymentFilterViewModel
// =============================================================================
// Provides properties for building filter forms and holding filtered results.
// Used in conjunction with the PaymentsController and Index view.
// =============================================================================
public class PaymentFilterViewModel
{
    // -------------------------------------------------------------------------
    // FILTER PARAMETERS
    // -------------------------------------------------------------------------
    // Query text to search within payment descriptions.
    public string? Query { get; set; }

    // Determines whether income payments are included in results.
    public bool ShowIncomes { get; set; } = true;

    // Determines whether expense payments are included in results.
    public bool ShowExpenses { get; set; } = true;

    // Selected member ID for filtering results (optional).
    public int? MemberId { get; set; }

    // Optional result limit (e.g., last 15 or 30 entries).
    public int? Limit { get; set; }

    // -------------------------------------------------------------------------
    // RESULT DATA
    // -------------------------------------------------------------------------
    // Provides the list of members for the filter dropdown.
    public IEnumerable<SelectListItem>? Members { get; set; }

    // Holds the filtered list of payments after applying the filters.
    public IEnumerable<Payment>? Payments { get; set; }
}
