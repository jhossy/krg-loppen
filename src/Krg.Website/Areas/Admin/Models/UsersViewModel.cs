using Microsoft.AspNetCore.Identity;

namespace Krg.Website.Areas.Admin.Models;

public class UsersViewModel
{
    public List<IdentityUser> Users { get; set; } = new List<IdentityUser>();
}