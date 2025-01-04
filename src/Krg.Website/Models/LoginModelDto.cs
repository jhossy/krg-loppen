using Microsoft.Build.Framework;

namespace Krg.Website.Models;

public class LoginModelDto
{
    [Required]
    public string Email { get; set; }

    public string Password { get; set; }
}