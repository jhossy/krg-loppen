namespace Krg.Website.Areas.Admin.Models;

public class CreateUserDto
{
    public required string Email { get; set; }

    public required string Password { get; set; }
    
    public required string RepeatPassword { get; set; }
}