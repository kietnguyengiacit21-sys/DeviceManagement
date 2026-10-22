using System.ComponentModel.DataAnnotations;
namespace DeviceManagement.Models;

public class DeviceCategory
{
    public int Id { get; set; }
    [Required, StringLength(60)][Display(Name = "Category name")] public string Name { get; set; } = "";
    [StringLength(250)] public string? Description { get; set; }
    public ICollection<Device> Devices { get; set; } = new List<Device>();
}
