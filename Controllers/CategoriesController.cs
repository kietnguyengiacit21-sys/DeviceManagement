using DeviceManagement.Data;
using DeviceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DeviceManagement.Controllers;

public class CategoriesController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.DeviceCategories.Include(x => x.Devices).OrderBy(x => x.Name).ToListAsync());
    public IActionResult Create() => View(new DeviceCategory());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(DeviceCategory x) { if (await db.DeviceCategories.AnyAsync(c => c.Name == x.Name)) ModelState.AddModelError("Name", "Category already exists."); if (!ModelState.IsValid) return View(x); db.Add(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int? id) { if (id == null) return NotFound(); var x = await db.DeviceCategories.FindAsync(id); return x == null ? NotFound() : View(x); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, DeviceCategory x) { if (id != x.Id) return NotFound(); if (await db.DeviceCategories.AnyAsync(c => c.Id != id && c.Name == x.Name)) ModelState.AddModelError("Name", "Category already exists."); if (!ModelState.IsValid) return View(x); db.Update(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Delete(int? id) { if (id == null) return NotFound(); var x = await db.DeviceCategories.Include(c => c.Devices).FirstOrDefaultAsync(c => c.Id == id); return x == null ? NotFound() : View(x); }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id) { var x = await db.DeviceCategories.Include(c => c.Devices).FirstOrDefaultAsync(c => c.Id == id); if (x == null) return NotFound(); if (x.Devices.Count > 0) { TempData["Error"] = "Cannot delete a category that still contains devices."; return RedirectToAction(nameof(Index)); } db.Remove(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
}
