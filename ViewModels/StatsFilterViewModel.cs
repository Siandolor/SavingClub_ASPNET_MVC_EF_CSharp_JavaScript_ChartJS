// =============================================================================
// FILE: StatsFilterViewModel.cs
// PROJECT: SavingsClub
// =============================================================================
// Provides filtering options for the statistics overview.
// Defines a date range and optional member filter used to
// refine statistical calculations and chart data.
// =============================================================================

using Microsoft.AspNetCore.Mvc.Rendering;

namespace SavingsClub.ViewModels;

// =============================================================================
// CLASS: StatsFilterViewModel
// =============================================================================
// Used by the StatsController and views to collect filter criteria for
// generating summary data, charts, and reports in the statistics section.
// =============================================================================
public class StatsFilterViewModel
{
    // -------------------------------------------------------------------------
    // FILTER PARAMETERS
    // -------------------------------------------------------------------------
    // Start date for the filtered statistics range.
    public DateTime? From { get; set; }

    // End date for the filtered statistics range.
    public DateTime? To { get; set; }

    // Selected member ID for filtering statistics (optional).
    public int? MemberId { get; set; }

    // -------------------------------------------------------------------------
    // SUPPORTING DATA
    // -------------------------------------------------------------------------
    // Collection of available members used to populate the filter dropdown.
    public IEnumerable<SelectListItem>? Members { get; set; }
}
