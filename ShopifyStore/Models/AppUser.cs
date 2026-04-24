using System.ComponentModel.DataAnnotations;

namespace ShopifyStore.Models;

public class AppUser
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; }
}
