using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Controllers;

public class LoginController(SignInManager<IdentityUser> signInManager) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        
        var loginResult = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, lockoutOnFailure: true);
        
        if (loginResult.Succeeded)
        {
            return RedirectToAction("Index", "Home", new { area = "Admin" });    
        }

        if (loginResult.IsLockedOut)
        {
            ModelState.AddModelError("Password", "Din konto er låst ude. Prøv igen om 10 min");            
        }
        
        ModelState.AddModelError("Password", "Invalid email or password. Please try again.");
        
        return View("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        
        return RedirectToAction("Index", "Home", new { area = "Admin" });
    }
}

public class LoginDto
{
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }
}