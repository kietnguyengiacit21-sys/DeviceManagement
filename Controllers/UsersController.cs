using DeviceManagement.Data;
using DeviceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DeviceManagement.Controllers;

public class UsersController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search) { ViewData["Search"] = search; var q = db.AppUsers.AsQueryable(); if (!string.IsNullOrWhiteSpace(search)) q = q.Where(x => x.FullName.Contains(search) || x.Email.Contains(search)); return View(await q.OrderBy(x => x.FullName).ToListAsync()); }
    public IActionResult Create() => View(new AppUser());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(AppUser x) { if (await db.AppUsers.AnyAsync(u => u.Email == x.Email)) ModelState.AddModelError("Email", "Email already exists."); if (!ModelState.IsValid) return View(x); db.Add(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int? id) { if (id == null) return NotFound(); var x = await db.AppUsers.FindAsync(id); return x == null ? NotFound() : View(x); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, AppUser x) { if (id != x.Id) return NotFound(); if (await db.AppUsers.AnyAsync(u => u.Id != id && u.Email == x.Email)) ModelState.AddModelError("Email", "Email already exists."); if (!ModelState.IsValid) return View(x); db.Update(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Delete(int? id) { if (id == null) return NotFound(); var x = await db.AppUsers.FindAsync(id); return x == null ? NotFound() : View(x); }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id) { var x = await db.AppUsers.Include(u => u.Devices).FirstOrDefaultAsync(u => u.Id == id); if (x == null) return NotFound(); if (x.Devices.Count > 0) { TempData["Error"] = "Reassign this user's devices before deleting the user."; return RedirectToAction(nameof(Index)); } db.Remove(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
}
