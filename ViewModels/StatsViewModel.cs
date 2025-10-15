// =============================================================================
// FILE: StatsViewModel.cs
// PROJECT: SavingsClub
// =============================================================================
// Combines all statistical data components for display in the statistics view.
// Includes overview totals, filtered results, chart data, and filter state.
// =============================================================================

namespace SavingsClub.ViewModels;

// =============================================================================
// CLASS: StatsViewModel
// =============================================================================
// Serves as the main container for data presented in the Stats/Index view.
// Aggregates both overall and filtered statistics, including top entries
// for visualization and user-selected filter information.
// =============================================================================
public class StatsViewModel
{
    // -------------------------------------------------------------------------
    // OVERVIEW DATA
    // -------------------------------------------------------------------------
    // Summary of all incomes, expenses, and balance across all payments.
    public StatsOverviewViewModel Overview { get; set; } = new();

    // Summary of data after applying date or member filters (optional).
    public StatsOverviewViewModel? Filtered { get; set; }

    // -------------------------------------------------------------------------
    // FILTER PARAMETERS
    // -------------------------------------------------------------------------
    // Contains the currently active filter criteria (dates, member selection).
    public StatsFilterViewModel Filter { get; set; } = new();

    // -------------------------------------------------------------------------
    // CHART DATA
    // -------------------------------------------------------------------------
    // Holds a mixed list of the top 5 income and expense items for chart display.
    public List<StatsChartItem> Top5Mixed { get; set; } = new();

    // -------------------------------------------------------------------------
    // VIEW STATE
    // -------------------------------------------------------------------------
    // Indicates whether any filter (date range or member) has been applied.
    public bool HasFilterApplied { get; set; }
}
