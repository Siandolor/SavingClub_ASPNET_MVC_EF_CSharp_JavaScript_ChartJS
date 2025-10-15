// =============================================================================
// FILE: StatsController.cs
// PROJECT: SavingsClub
// =============================================================================
// Provides analytical overviews and chart data for all financial transactions.
// Handles filtering by date range and member, aggregation of totals, and
// generation of top-5 income and expense statistics.
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsClub.Data;
using SavingsClub.Models;
using SavingsClub.ViewModels;

namespace SavingsClub.Controllers;

public class StatsController : Controller
{
    // -------------------------------------------------------------------------
    // DEPENDENCIES
    // -------------------------------------------------------------------------
    // _db: Database context providing access to payment and member data.
    // -------------------------------------------------------------------------
    private readonly AppDbContext _db;


    // -------------------------------------------------------------------------
    // CONSTRUCTOR
    // -------------------------------------------------------------------------
    // Injects database context for use in queries and aggregation logic.
    // -------------------------------------------------------------------------
    public StatsController(AppDbContext db)
    {
        _db = db;
    }


    // -------------------------------------------------------------------------
    // ACTION: INDEX
    // -------------------------------------------------------------------------
    // Displays the statistics dashboard.
    // Features:
    //  - Global overview of all payments
    //  - Filtered statistics by date and/or member
    //  - Top 5 incomes and top 5 expenses visualization
    // Populates a combined StatsVM for rendering in the view.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Index(DateTime? from, DateTime? to, int? memberId)
    {
        var all = _db.Payments.AsNoTracking();
        var overview = await BuildOverview(all);

        var filterQuery = _db.Payments.AsNoTracking()
            .Include(p => p.Member)
            .AsQueryable();

        if (from.HasValue)
            filterQuery = filterQuery.Where(p => p.Date.Date >= from.Value.Date);

        if (to.HasValue)
            filterQuery = filterQuery.Where(p => p.Date.Date <= to.Value.Date);

        if (memberId.HasValue)
            filterQuery = filterQuery.Where(p => p.MemberId == memberId.Value);

        var filteredList = await filterQuery.ToListAsync();
        var filtered = await BuildOverview(filteredList.AsQueryable());

        var topIncome = filteredList
            .Where(p => p.IsIncome)
            .OrderByDescending(p => p.Amount)
            .Take(5)
            .Select(p => new StatsChartItem
            {
                Label = $"{p.Member!.FullName} – {p.Description}",
                Value = p.Amount
            })
            .ToList();

        var topExpense = filteredList
            .Where(p => !p.IsIncome)
            .OrderByDescending(p => p.Amount)
            .Take(5)
            .Select(p => new StatsChartItem
            {
                Label = $"{p.Member!.FullName} – {p.Description}",
                Value = -p.Amount
            })
            .ToList();

        var vm = new StatsViewModel
        {
            Overview = overview,
            Filtered = filtered,
            HasFilterApplied = from.HasValue || to.HasValue || memberId.HasValue,
            Top5Mixed = topIncome.Concat(topExpense).ToList(),
            Filter = new StatsFilterViewModel
            {
                From = from,
                To = to,
                MemberId = memberId,
                Members = await _db.Members
                    .OrderBy(m => m.LastName)
                    .ThenBy(m => m.FirstName)
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.FullName
                    })
                    .ToListAsync()
            }
        };

        return View(vm);
    }


    // -------------------------------------------------------------------------
    // ACTION: CHARTDATA (AJAX)
    // -------------------------------------------------------------------------
    // Returns top-5 income and expense entries as JSON for chart rendering.
    // The returned data combines both lists into a single unified dataset.
    // -------------------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> ChartData(DateTime? from, DateTime? to, int? memberId)
    {
        var query = _db.Payments.Include(p => p.Member).AsQueryable();

        if (from.HasValue)
            query = query.Where(p => p.Date.Date >= from.Value.Date);

        if (to.HasValue)
            query = query.Where(p => p.Date.Date <= to.Value.Date);

        if (memberId.HasValue)
            query = query.Where(p => p.MemberId == memberId.Value);

        var list = await query.ToListAsync();

        var topIncome = list
            .Where(p => p.IsIncome)
            .OrderByDescending(p => p.Amount)
            .Take(5)
            .Select(p => new
            {
                Label = $"{p.Member!.FullName} – {p.Description}",
                Value = p.Amount
            });

        var topExpense = list
            .Where(p => !p.IsIncome)
            .OrderByDescending(p => p.Amount)
            .Take(5)
            .Select(p => new
            {
                Label = $"{p.Member!.FullName} – {p.Description}",
                Value = -p.Amount
            });

        return Json(topIncome.Concat(topExpense));
    }


    // -------------------------------------------------------------------------
    // HELPER: BuildOverview
    // -------------------------------------------------------------------------
    // Aggregates totals from a payment query.
    // Calculates:
    //  - Total income and expense counts
    //  - Sum of all income and expense values
    // Used both for full and filtered datasets.
    // -------------------------------------------------------------------------
    private static async Task<StatsOverviewViewModel> BuildOverview(IQueryable<Payment> query)
    {
        return await Task.Run(() =>
        {
            var incomes = query.Where(p => p.IsIncome).ToList();
            var expenses = query.Where(p => !p.IsIncome).ToList();

            return new StatsOverviewViewModel
            {
                CountIncome = incomes.Count,
                CountExpense = expenses.Count,
                SumIncome = incomes.Sum(p => p.Amount),
                SumExpense = expenses.Sum(p => p.Amount)
            };
        });
    }
}
