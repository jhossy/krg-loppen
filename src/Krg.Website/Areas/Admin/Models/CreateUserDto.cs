using System.ComponentModel.DataAnnotations;

namespace Krg.Website.Areas.Admin.Models;

public class CreateUserDto
{
    [EmailAddress]
    public required string Email { get; set; }

    public required string Password { get; set; }
    
    public required string RepeatPassword { get; set; }
}