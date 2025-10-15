// =============================================================================
// FILE: MembersController.cs
// PROJECT: SavingsClub
// =============================================================================
// Handles CRUD operations for Member entities, including image upload and
// validation logic. Provides functionality to create, edit, list, and delete
// members within the SavingsClub application.
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavingsClub.Data;
using SavingsClub.Models;

namespace SavingsClub.Controllers;

public class MembersController : Controller
{
    // -------------------------------------------------------------------------
    // DEPENDENCIES
    // -------------------------------------------------------------------------
    // _db: Database context providing access to member and payment entities.
    // _env: Provides environment details for file system operations (e.g., image upload path).
    // -------------------------------------------------------------------------
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;


    // -------------------------------------------------------------------------
    // CONSTRUCTOR
    // -------------------------------------------------------------------------
    // Injects database context and hosting environment.
    // -------------------------------------------------------------------------
    public MembersController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }


    // -------------------------------------------------------------------------
    // ACTION: INDEX
    // -------------------------------------------------------------------------
    // Displays a list of all members, ordered by last and first name.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Index()
    {
        var members = await _db.Members
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync();

        return View(members);
    }


    // -------------------------------------------------------------------------
    // ACTION: CREATE (GET)
    // -------------------------------------------------------------------------
    // Displays the empty member creation form.
    // -------------------------------------------------------------------------
    public IActionResult Create() => View(new Member());


    // -------------------------------------------------------------------------
    // ACTION: CREATE (POST)
    // -------------------------------------------------------------------------
    // Handles new member creation and optional profile image upload.
    // Uploaded files are saved in /wwwroot/images/members with unique names.
    // -------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Member member, IFormFile? imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            var uploads = Path.Combine(_env.WebRootPath, "images", "members");
            Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var path = Path.Combine(uploads, fileName);

            using var stream = System.IO.File.Create(path);
            await imageFile.CopyToAsync(stream);

            member.ImagePath = $"/images/members/{fileName}";
        }

        if (!ModelState.IsValid)
            return View(member);

        _db.Members.Add(member);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // -------------------------------------------------------------------------
    // ACTION: EDIT (GET)
    // -------------------------------------------------------------------------
    // Displays the edit form for the selected member.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Edit(int id)
    {
        var member = await _db.Members.FindAsync(id);
        if (member == null)
            return NotFound();

        return View(member);
    }


    // -------------------------------------------------------------------------
    // ACTION: EDIT (POST)
    // -------------------------------------------------------------------------
    // Updates existing member data and replaces profile image if a new file
    // is uploaded. Retains previous data if no new image is provided.
    // -------------------------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Member input, IFormFile? imageFile)
    {
        var member = await _db.Members.FindAsync(id);
        if (member == null)
            return NotFound();

        if (!ModelState.IsValid)
            return View(input);

        member.FirstName = input.FirstName;
        member.LastName = input.LastName;
        member.IsActive = input.IsActive;

        if (imageFile != null && imageFile.Length > 0)
        {
            var uploads = Path.Combine(_env.WebRootPath, "images", "members");
            Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var path = Path.Combine(uploads, fileName);

            using var stream = System.IO.File.Create(path);
            await imageFile.CopyToAsync(stream);

            member.ImagePath = $"/images/members/{fileName}";
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    // -------------------------------------------------------------------------
    // ACTION: DELETE (GET)
    // -------------------------------------------------------------------------
    // Displays the confirmation view before member deletion.
    // -------------------------------------------------------------------------
    public async Task<IActionResult> Delete(int id)
    {
        var member = await _db.Members.FindAsync(id);
        if (member == null)
            return NotFound();

        return View(member);
    }


    // -------------------------------------------------------------------------
    // ACTION: DELETE (POST)
    // -------------------------------------------------------------------------
    // Confirms and executes member deletion if no related payments exist.
    // If payments are linked, deletion is blocked and an error message displayed.
    // -------------------------------------------------------------------------
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var member = await _db.Members
            .Include(m => m.Payments)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (member == null)
            return NotFound();

        if (member.Payments.Any())
        {
            TempData["Error"] = "Deletion not possible: This member has existing payments. Please mark the member as inactive instead.";
            return RedirectToAction(nameof(Index));
        }

        _db.Members.Remove(member);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
