using System.ComponentModel.DataAnnotations;

namespace Krg.Website.Areas.Admin.Models;

public class EditProfileDto
{
    [Required]
    public required string Email { get; init; }
    
    [Required]
    public required string CurrentPassword { get; init; }
    
    [Required]
    public required string NewPassword { get; init; }
    
    [Required]
    public required string RepeatNewPassword { get; init; }
}