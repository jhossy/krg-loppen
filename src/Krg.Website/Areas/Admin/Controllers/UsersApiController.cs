using Krg.Website.Areas.Admin.Models;
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
            logger.LogInformation($"ResetPassword for {resetPasswordDto.Email}");

            var user = await signInManager.UserManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user == null)
            {
                logger.LogError($"User could not be found using email: {resetPasswordDto.Email}");

                return new JsonResult("User does not exist with the provided email.");
            }

            var resetToken = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

            string newPassword = Guid.NewGuid().ToString();

            await signInManager.UserManager.ResetPasswordAsync(user, resetToken, newPassword);

            return new JsonResult($"Password successfully reset to {newPassword}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ResetPassword failed for {resetPasswordDto.Email}");
        }
        return new JsonResult($"ResetPassword failed for {resetPasswordDto.Email}");
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

                return new JsonResult(new { users = signInManager.UserManager.Users.ToList() });
            }

            await signInManager.UserManager.DeleteAsync(user);

            return new JsonResult(new { users = signInManager.UserManager.Users.ToList() });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"DeleteUser failed for {deleteUserDto.Id}");
        }
        return new JsonResult(new { users = signInManager.UserManager.Users.ToList() });
    }
}