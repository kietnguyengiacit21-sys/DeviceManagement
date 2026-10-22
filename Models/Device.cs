using System.ComponentModel.DataAnnotations;
namespace DeviceManagement.Models;

public class Device
{
    public int Id { get; set; }
    [Required, StringLength(100)][Display(Name = "Device name")] public string Name { get; set; } = "";
    [Required, StringLength(30), RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessage = "Use letters, numbers, underscore or hyphen only.")][Display(Name = "Device code")] public string Code { get; set; } = "";
    [Required, Display(Name = "Category")] public int CategoryId { get; set; }
    public DeviceCategory? Category { get; set; }
    [Required] public DeviceStatus Status { get; set; } = DeviceStatus.InUse;
    [DataType(DataType.Date), Display(Name = "Date of entry")] public DateTime DateOfEntry { get; set; } = DateTime.Today;
    [Display(Name = "Assigned user")] public int? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
