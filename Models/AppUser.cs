using System.ComponentModel.DataAnnotations;
namespace DeviceManagement.Models;

public class AppUser
{
    public int Id { get; set; }
    [Required, StringLength(100)][Display(Name = "Full name")] public string FullName { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
    [Required, Phone, StringLength(25)][Display(Name = "Phone number")] public string PhoneNumber { get; set; } = "";
    public ICollection<Device> Devices { get; set; } = new List<Device>();
}
