using Krg.Website.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

public class UsersController(SignInManager<IdentityUser> signInManager, ILogger<ProfileController> logger) : BaseAdminController
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        logger.LogInformation($"Creating user with email: {createUserDto.Email}");
        
        var user = await signInManager.UserManager.FindByEmailAsync(createUserDto.Email);
        if (user != null)
        {
            logger.LogError($"User with email: {createUserDto.Email} already exists.");
            
            ModelState.AddModelError("Email", $"User with email: {createUserDto.Email} already exists.");
            
            return View("Index");
        }
        
        if (!string.Equals(createUserDto.Password, createUserDto.RepeatPassword))
        {
            logger.LogError("Password and repeat password does not match");
            
            ModelState.AddModelError("RepeatPassword", "'Password' and 'repeat' password does not match");
            
            return View("Index");
        }
        
        user = new IdentityUser(createUserDto.Email) {Email = createUserDto.Email};
        
        var identityResult = await signInManager.UserManager.CreateAsync(user, createUserDto.Password);
        if (!identityResult.Succeeded)
        {
            logger.LogError($"The user could not be created {string.Join(",", identityResult.Errors.Select(x => x.Description))}");

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("Summary", error.Description);
            }
            return View("Index");
        }
        
        ViewData["Message"] = "User successfully created.";
        
        return View("Index");
    }
}