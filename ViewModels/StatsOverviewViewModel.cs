// =============================================================================
// FILE: StatsOverviewViewModel.cs
// PROJECT: SavingsClub
// =============================================================================
// Represents summarized statistical data for all payments.
// Includes total counts and sums for incomes and expenses,
// as well as derived properties for total count and balance.
// =============================================================================

namespace SavingsClub.ViewModels;

// =============================================================================
// CLASS: StatsOverviewViewModel
// =============================================================================
// Used to display aggregated statistics in the overview and
// analytics sections (e.g., total income, total expenses, balance).
// =============================================================================
public class StatsOverviewViewModel
{
    // -------------------------------------------------------------------------
    // PAYMENT COUNTS
    // -------------------------------------------------------------------------
    // Total number of income transactions.
    public int CountIncome { get; set; }

    // Total number of expense transactions.
    public int CountExpense { get; set; }

    // Computed total number of all transactions.
    public int CountAll => CountIncome + CountExpense;

    // -------------------------------------------------------------------------
    // PAYMENT SUMS
    // -------------------------------------------------------------------------
    // Total sum of all incomes.
    public decimal SumIncome { get; set; }

    // Total sum of all expenses.
    public decimal SumExpense { get; set; }

    // Computed financial balance (SumIncome - SumExpense).
    public decimal Balance => SumIncome - SumExpense;
}
