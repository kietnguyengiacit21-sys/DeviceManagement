using DeviceManagement.Models;
using Microsoft.EntityFrameworkCore;
namespace DeviceManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<DeviceCategory> DeviceCategories => Set<DeviceCategory>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<DeviceCategory>().HasIndex(x => x.Name).IsUnique();
        b.Entity<Device>().HasIndex(x => x.Code).IsUnique();
        b.Entity<Device>().HasOne(x => x.Category).WithMany(x => x.Devices).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
    }
}
