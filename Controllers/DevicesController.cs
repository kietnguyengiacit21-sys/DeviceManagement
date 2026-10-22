using DeviceManagement.Data;
using DeviceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace DeviceManagement.Controllers;

public class DevicesController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, DeviceStatus? status, int? categoryId)
    {
        ViewData["Search"] = search; ViewData["Status"] = status; ViewData["CategoryId"] = categoryId;
        ViewBag.Categories = new SelectList(await db.DeviceCategories.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", categoryId);
        ViewBag.Statuses = new SelectList(Enum.GetValues<DeviceStatus>().Select(x => new { Id = x, Name = Label(x) }), "Id", "Name", status);
        var q = db.Devices.Include(x => x.Category).Include(x => x.AppUser).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(x => x.Name.Contains(search) || x.Code.Contains(search));
        if (status.HasValue) q = q.Where(x => x.Status == status.Value);
        if (categoryId.HasValue) q = q.Where(x => x.CategoryId == categoryId.Value);
        return View(await q.OrderBy(x => x.Name).ToListAsync());
    }
    public async Task<IActionResult> Create() { await Options(); return View(new Device()); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Device item)
    {
        if (await db.Devices.AnyAsync(x => x.Code == item.Code)) ModelState.AddModelError("Code", "Device code already exists.");
        if (!ModelState.IsValid) { await Options(item.CategoryId, item.AppUserId); return View(item); }
        db.Devices.Add(item); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int? id) { if (id == null) return NotFound(); var x = await db.Devices.FindAsync(id); if (x == null) return NotFound(); await Options(x.CategoryId, x.AppUserId); return View(x); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Device item)
    {
        if (id != item.Id) return NotFound();
        if (await db.Devices.AnyAsync(x => x.Id != id && x.Code == item.Code)) ModelState.AddModelError("Code", "Device code already exists.");
        if (!ModelState.IsValid) { await Options(item.CategoryId, item.AppUserId); return View(item); }
        db.Devices.Update(item); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id) { if (id == null) return NotFound(); var x = await db.Devices.Include(x => x.Category).Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id); return x == null ? NotFound() : View(x); }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id) { var x = await db.Devices.FindAsync(id); if (x == null) return NotFound(); db.Devices.Remove(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
    private async Task Options(int? category = null, int? user = null) { ViewBag.Categories = new SelectList(await db.DeviceCategories.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", category); ViewBag.Users = new SelectList(await db.AppUsers.OrderBy(x => x.FullName).ToListAsync(), "Id", "FullName", user); ViewBag.Statuses = new SelectList(Enum.GetValues<DeviceStatus>().Select(x => new { Id = x, Name = Label(x) }), "Id", "Name"); }
    public static string Label(DeviceStatus x) => x switch { DeviceStatus.InUse => "In use", DeviceStatus.Broken => "Broken", DeviceStatus.UnderMaintenance => "Under maintenance", _ => x.ToString() };
}
