// =============================================================================
// FILE: StatsChartItem.cs
// PROJECT: SavingsClub
// =============================================================================
// Represents a single data point for use in statistical charts.
// Each item defines a label (e.g., member name and description)
// and a corresponding numeric value.
// =============================================================================

namespace SavingsClub.ViewModels;

// =============================================================================
// CLASS: StatsChartItem
// =============================================================================
// Used primarily by the StatsController and chart visualizations to structure
// top-5 income and expense data for display in statistical overviews.
// =============================================================================
public class StatsChartItem
{
    // -------------------------------------------------------------------------
    // PROPERTIES
    // -------------------------------------------------------------------------
    // The label displayed on the chart (e.g., "John Doe – Donation #5").
    public string Label { get; set; } = string.Empty;

    // The numeric value represented by the chart bar or slice.
    public decimal Value { get; set; }
}
