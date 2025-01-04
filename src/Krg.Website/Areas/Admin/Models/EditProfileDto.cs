using System.ComponentModel.DataAnnotations;

namespace Krg.Website.Areas.Admin.Models;

public class EditProfileDto
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string CurrentPassword { get; set; }
    
    [Required]
    public string NewPassword { get; set; }
    
    [Required]
    public string RepeatNewPassword { get; set; }
}