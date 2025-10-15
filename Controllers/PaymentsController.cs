// =============================================================================
// FILE: PaymentsController.cs
// PROJECT: SavingsClub
// =============================================================================
// Manages CRUD operations for payment entries, including filtering, validation,
// and association with members. Supports search, filtering by income/expense,
// and pagination for better performance and clarity in the dashboard view.
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsClub.Data;
using SavingsClub.Models;
using SavingsClub.ViewModels;

namespace SavingsClub.Controllers;

public class PaymentsController : Controller
{
    // -------------------------------------------------------------------------
    // DEPENDENCIES
    // -------------------------------------------------------------------------
    // _db: Database context providing access to payments and members.
    // -------------------------------------------------------------------------
    private readonly AppDbContext _db;


    // -------------------------------------------------------------------------
    // CONSTRUCTOR
    // -------------------------------------------------------------------------
    // Injects database context for persistence and querying.
    // -------------------------------------------------------------------------
    public PaymentsController(AppDbContext db)
    {
        _db = db;
    }


    // -------------------------------------------------------------------------
    // ACTION: INDEX
    // -------------------------------------------------------------------------
    // Displays a filtered and optionally limited list of payments.
    // Supports filtering by:
    //  - Search term (in Description)
    //  - Member ID
    //  - Income/Expense flag
    //  - Page size (number of entries)
    // Builds a view model containing payments and filter state.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Index(string? searchTerm, int? memberId, bool? isIncome, int? pageSize)
    {
        var query = _db.Payments.Include(p => p.Member).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p => p.Description.Contains(searchTerm));

        if (memberId.HasValue)
            query = query.Where(p => p.MemberId == memberId);

        if (isIncome.HasValue)
            query = query.Where(p => p.IsIncome == isIncome.Value);

        query = query.OrderByDescending(p => p.Date).ThenByDescending(p => p.Id);

        if (pageSize.HasValue && pageSize.Value > 0)
            query = query.Take(pageSize.Value);

        var payments = await query.ToListAsync();

        var members = await _db.Members
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.FullName
            })
            .ToListAsync();

        var vm = new PaymentsIndexViewModel
        {
            Payments = payments,
            SearchTerm = searchTerm,
            MemberId = memberId,
            IsIncome = isIncome,
            PageSize = pageSize ?? 0,
            Members = members
        };

        return View(vm);
    }


    // -------------------------------------------------------------------------
    // ACTION: CREATE (GET)
    // -------------------------------------------------------------------------
    // Displays the empty form to create a new payment.
    // Prepares member selection list for assignment.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Create()
    {
        await PopulateMembersSelectList();
        return View(new Payment { Date = DateTime.Today, IsIncome = true });
    }


    // -------------------------------------------------------------------------
    // ACTION: CREATE (POST)
    // -------------------------------------------------------------------------
    // Validates and saves a new payment record.
    // Ensures:
    //  - Date is not in the future
    //  - Member exists and is active
    // -------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Payment payment)
    {
        await ValidatePaymentAsync(payment);

        if (!ModelState.IsValid)
        {
            await PopulateMembersSelectList();
            return View(payment);
        }

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }


    // -------------------------------------------------------------------------
    // ACTION: EDIT (GET)
    // -------------------------------------------------------------------------
    // Displays an edit form for the selected payment.
    // Includes inactive members for historic data consistency.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Edit(int id)
    {
        var p = await _db.Payments.FindAsync(id);
        if (p == null)
            return NotFound();

        await PopulateMembersSelectList(includeInactive: true);
        return View(p);
    }


    // -------------------------------------------------------------------------
    // ACTION: EDIT (POST)
    // -------------------------------------------------------------------------
    // Updates an existing payment and revalidates the data.
    // Allows editing for inactive members but prevents future dates.
    // -------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Payment payment)
    {
        var existing = await _db.Payments.FindAsync(id);
        if (existing == null)
            return NotFound();

        await ValidatePaymentAsync(payment, allowInactiveMember: true);

        if (!ModelState.IsValid)
        {
            await PopulateMembersSelectList(includeInactive: true);
            return View(payment);
        }

        existing.Amount = payment.Amount;
        existing.IsIncome = payment.IsIncome;
        existing.Description = payment.Description;
        existing.Date = payment.Date;
        existing.MemberId = payment.MemberId;

        await _db.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }


    // -------------------------------------------------------------------------
    // ACTION: DELETE (GET)
    // -------------------------------------------------------------------------
    // Displays confirmation page before deleting a payment.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Payments
            .Include(p => p.Member)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (item == null)
            return NotFound();

        return View(item);
    }


    // -------------------------------------------------------------------------
    // ACTION: DELETE (POST)
    // -------------------------------------------------------------------------
    // Confirms and executes payment deletion.
    // Redirects back to main list after successful removal.
    // -------------------------------------------------------------------------
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.Payments.FindAsync(id);

        if (item != null)
        {
            _db.Payments.Remove(item);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Home");
    }


    // -------------------------------------------------------------------------
    // HELPER: PopulateMembersSelectList
    // -------------------------------------------------------------------------
    // Builds member dropdown for selection in create/edit forms.
    // Optionally includes inactive members.
    // -------------------------------------------------------------------------
    private async Task PopulateMembersSelectList(bool includeInactive = false)
    {
        var q = _db.Members.AsQueryable();

        if (!includeInactive)
            q = q.Where(m => m.IsActive);

        var list = await q
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.FullName
            })
            .ToListAsync();

        ViewBag.MemberId = list;
    }


    // -------------------------------------------------------------------------
    // HELPER: ValidatePaymentAsync
    // -------------------------------------------------------------------------
    // Performs domain-level validation for a payment record.
    // Ensures:
    //  - Date is not in the future
    //  - Member exists
    //  - Inactive members cannot receive new payments (unless editing)
    // -------------------------------------------------------------------------
    private async Task ValidatePaymentAsync(Payment payment, bool allowInactiveMember = false)
    {
        if (payment.Date.Date > DateTime.Today)
            ModelState.AddModelError(nameof(payment.Date), "Datum darf nicht in der Zukunft liegen.");

        var member = await _db.Members.FindAsync(payment.MemberId);

        if (member == null)
        {
            ModelState.AddModelError(nameof(payment.MemberId), "Mitglied existiert nicht.");
        }
        else if (!allowInactiveMember && !member.IsActive)
        {
            ModelState.AddModelError(nameof(payment.MemberId),
                "Für inaktive Mitglieder dürfen keine neuen Zahlungen erfasst werden.");
        }
    }
}
