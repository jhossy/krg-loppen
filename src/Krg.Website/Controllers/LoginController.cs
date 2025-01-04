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
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        var loginResult = await signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, lockoutOnFailure: false);
        
        if (loginResult.Succeeded)
        {
            return RedirectToAction("Index", "Home", new { area = "Admin" });    
        }
        
        return RedirectToAction(nameof(Index));
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
    public string Username { get; set; }

    public string Password { get; set; }
}