using Krg.Website.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

public class UsersController(SignInManager<IdentityUser> signInManager, ILogger<ProfileController> logger) : BaseAdminController
{
    public IActionResult Index()
    {
        var viewModel = new UsersViewModel
        {
            Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList()
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        logger.LogInformation($"Creating user with email: {createUserDto.Email}");

        var usersViewModel = new UsersViewModel { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
        
        var user = await signInManager.UserManager.FindByEmailAsync(createUserDto.Email);
        if (user != null)
        {
            logger.LogError($"User with email: {createUserDto.Email} already exists.");
            
            ModelState.AddModelError("Email", $"User with email: {createUserDto.Email} already exists.");
            
            return View("Index", usersViewModel);
        }
        
        if (!string.Equals(createUserDto.Password, createUserDto.RepeatPassword))
        {
            logger.LogError("Password and repeat password does not match");
            
            ModelState.AddModelError("RepeatPassword", "'Password' and 'repeat password' does not match");
            
            return View("Index", usersViewModel);
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
            return View("Index", usersViewModel);
        }
        
        return RedirectToAction("Index");
    }
}