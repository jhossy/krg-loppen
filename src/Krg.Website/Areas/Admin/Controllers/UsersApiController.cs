using Krg.Website.Areas.Admin.Models;
using Krg.Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class UsersApiController(SignInManager<IdentityUser> signInManager, ILogger<UsersApiController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto resetPasswordDto)
    {
        try
        {
            logger.LogInformation($"ResetPassword for {resetPasswordDto.Id}");

            var user = await signInManager.UserManager.FindByIdAsync(resetPasswordDto.Id);

            if (user == null)
            {
                logger.LogError($"User could not be found using Id: {resetPasswordDto.Id}");

                return new JsonResult("User does not exist with the provided Id.");
            }

            var resetToken = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

            string newPassword = PasswordGenerator.GenerateRandomPassword();

            var result = await signInManager.UserManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                return new JsonResult(new { message = $"Password successfully reset to {newPassword}" });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ResetPassword failed for {resetPasswordDto.Id}");
        }
        return new JsonResult(new { message = $"ResetPassword failed for {resetPasswordDto.Id}" });
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteUser([FromBody]DeleteUserDto deleteUserDto)
    {
        try
        {
            logger.LogInformation($"DeleteUser for {deleteUserDto.Id}");

            var user = await signInManager.UserManager.FindByIdAsync(deleteUserDto.Id);

            if (user == null)
            {
                logger.LogError($"User could not be found using id: {deleteUserDto.Id}");

                return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
            }

            var currentUser = await signInManager.UserManager.GetUserAsync(User);
            if (currentUser != null && user.Id == currentUser.Id)
            {
                logger.LogError("Cannot delete currently logged in user");

                return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
            }

            await signInManager.UserManager.DeleteAsync(user);

            return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"DeleteUser failed for {deleteUserDto.Id}");
        }
        return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
    }

    public async Task<IActionResult> Deactivate([FromBody] UserDto userDto)
    {
        try
        {
            logger.LogInformation($"Deactivate for {userDto.Id}");

            var user = await signInManager.UserManager.FindByIdAsync(userDto.Id);

            if (user == null)
            {
                logger.LogError($"User could not be found using id: {userDto.Id}");

                return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
            }

            var currentUser = await signInManager.UserManager.GetUserAsync(User);
            if (currentUser != null && user.Id == currentUser.Id)
            {
                logger.LogError("Cannot deactivate currently logged in user");

                return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
            }

            await signInManager.UserManager.SetLockoutEnabledAsync(user, true);
            await signInManager.UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            
            return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Deactivate failed for {userDto.Id}");
        }
        return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
    }
    
    public async Task<IActionResult> Activate([FromBody] UserDto userDto)
    {
        try
        {
            logger.LogInformation($"Activate for {userDto.Id}");

            var user = await signInManager.UserManager.FindByIdAsync(userDto.Id);

            if (user == null)
            {
                logger.LogError($"User could not be found using id: {userDto.Id}");

                return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
            }

            var currentUser = await signInManager.UserManager.GetUserAsync(User);
            if (currentUser != null && user.Id == currentUser.Id)
            {
                logger.LogError("Cannot activate currently logged in user");

                return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
            }

            await signInManager.UserManager.SetLockoutEnabledAsync(user, false);
            await signInManager.UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(-1));
            
            return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Activate failed for {userDto.Id}");
        }
        return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
    }
}

public class UserDto
{
    public string Id { get; set; }
}