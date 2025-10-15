// =============================================================================
// FILE: PaymentsIndexViewModel.cs
// PROJECT: SavingsClub
// =============================================================================
// ViewModel for displaying and filtering payment entries in the main index view.
// Contains filter parameters, selected values, and result data collections.
// =============================================================================

using Microsoft.AspNetCore.Mvc.Rendering;
using SavingsClub.Models;

namespace SavingsClub.ViewModels;

// =============================================================================
// CLASS: PaymentsIndexViewModel
// =============================================================================
// Encapsulates filter options, selection lists, and resulting payment data
// for the Payments or Home index view. Designed for easy data binding.
// =============================================================================
public class PaymentsIndexViewModel
{
    // -------------------------------------------------------------------------
    // RESULT DATA
    // -------------------------------------------------------------------------
    // Collection of all payment entries returned after applying filters.
    public List<Payment> Payments { get; set; } = new();

    // -------------------------------------------------------------------------
    // FILTER PARAMETERS
    // -------------------------------------------------------------------------
    // Text to search within payment descriptions.
    public string? SearchTerm { get; set; }

    // Selected member ID for filtering payments.
    public int? MemberId { get; set; }

    // Defines whether to filter for incomes (true), expenses (false), or both (null).
    public bool? IsIncome { get; set; }

    // Defines the number of results to display (e.g., 15, 30, or all).
    public int PageSize { get; set; } = 0;

    // -------------------------------------------------------------------------
    // SELECTION LISTS
    // -------------------------------------------------------------------------
    // Provides the dropdown list of members for the view.
    public List<SelectListItem> Members { get; set; } = new();
}
