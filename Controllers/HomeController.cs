// =============================================================================
// FILE: HomeController.cs
// PROJECT: SavingsClub
// =============================================================================
// Primary controller handling the main application views and filtering logic.
// Includes data querying, filtering, and dynamic view preparation for the
// dashboard (Index) and static informational pages.
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsClub.Data;
using SavingsClub.Models;

namespace SavingsClub.Controllers;

public class HomeController : Controller
{
    // -------------------------------------------------------------------------
    // DEPENDENCIES
    // -------------------------------------------------------------------------
    // _db: Provides access to database entities via EF Core.
    // _logger: Used for diagnostic and runtime logging.
    // -------------------------------------------------------------------------
    private readonly AppDbContext _db;
    private readonly ILogger<HomeController> _logger;


    // -------------------------------------------------------------------------
    // CONSTRUCTOR
    // -------------------------------------------------------------------------
    // Injects application database context and logger service.
    // -------------------------------------------------------------------------
    public HomeController(AppDbContext db, ILogger<HomeController> logger)
    {
        _db = db;
        _logger = logger;
    }


    // -------------------------------------------------------------------------
    // ACTION: INDEX
    // -------------------------------------------------------------------------
    // Displays the transaction overview page.
    // Supports optional filters for:
    // - Search text (Description)
    // - Transaction type (Income/Expense)
    // - Member selection
    // - Limit (recent 15 or 30 items)
    // Populates dropdown lists via ViewBag.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Index(string? search, bool? isIncome, int? memberId, int? limit)
    {
        var query = _db.Payments
            .Include(p => p.Member)
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Description.Contains(search));

        if (isIncome.HasValue)
            query = query.Where(p => p.IsIncome == isIncome.Value);

        if (memberId.HasValue)
            query = query.Where(p => p.MemberId == memberId.Value);

        if (limit == 15)
            query = query.Take(15);
        else if (limit == 30)
            query = query.Take(30);

        var members = await _db.Members
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync();

        ViewBag.IsIncomeList = new SelectList(new[]
        {
            new { Value = "", Text = "⟨ alle ⟩" },
            new { Value = "true", Text = "nur Einnahmen" },
            new { Value = "false", Text = "nur Ausgaben" }
        }, "Value", "Text", isIncome?.ToString().ToLower());

        ViewBag.MemberList = new SelectList(members, "Id", "FullName", memberId);

        ViewBag.LimitList = new SelectList(new[]
        {
            new { Value = "", Text = "alle" },
            new { Value = "15", Text = "letzte 15" },
            new { Value = "30", Text = "letzte 30" }
        }, "Value", "Text", limit?.ToString());

        ViewBag.Search = search;

        var items = await query.ToListAsync();

        return View(items);
    }


    // -------------------------------------------------------------------------
    // ACTION: PRIVACY
    // -------------------------------------------------------------------------
    // Returns a static Privacy view (imprint/legal info).
    // -------------------------------------------------------------------------
    public IActionResult Privacy()
    {
        return View();
    }


    // -------------------------------------------------------------------------
    // ACTION: ERROR
    // -------------------------------------------------------------------------
    // Returns a detailed error view for unhandled exceptions or diagnostics.
    // Response cache disabled to prevent stale output.
    // -------------------------------------------------------------------------
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
