using Krg.Website.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

public class ProfileController(SignInManager<IdentityUser> signInManager, ILogger<ProfileController> logger) : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult EditProfile()
    {
        return View("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(EditProfileDto editProfileDto)
    {
        logger.LogInformation($"User edited profile for {editProfileDto.Email}");
        
        var user = await signInManager.UserManager.FindByEmailAsync(editProfileDto.Email);
        if (user == null)
        {
            logger.LogError($"User could not be found using email: {editProfileDto.Email}");
            
            ModelState.AddModelError("Email", "User does not exist with the provided email.");
            
            return View("Index");
        }

        if (!await signInManager.UserManager.CheckPasswordAsync(user, editProfileDto.CurrentPassword))
        {
            logger.LogError($"Invalid password specified for: {editProfileDto.Email}");
            
            ModelState.AddModelError("CurrentPassword", "Invalid password specified.");
            
            return View("Index");
        }

        if (!string.Equals(editProfileDto.NewPassword, editProfileDto.RepeatNewPassword))
        {
            logger.LogError("New and repeat password does not match");
            
            ModelState.AddModelError("RepeatNewPassword", "'New' and 'repeat' password does not match");
            
            return View("Index");
        }
        
        await signInManager.UserManager.ChangePasswordAsync(user, editProfileDto.CurrentPassword, editProfileDto.NewPassword);
            
        ViewData["Message"] = "Password successfully changed.";
        
        return View("Index");
    }
}