using Krg.Website.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Krg.Website.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProfileApiController(SignInManager<IdentityUser> signInManager, ILogger<ProfileApiController> logger) : ControllerBase
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
}