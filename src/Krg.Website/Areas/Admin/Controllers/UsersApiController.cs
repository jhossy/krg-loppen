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
    [HttpGet]
    public IActionResult GetAll()
    {
        return new JsonResult(new { users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() });
    }
    
    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody]UserDto resetPasswordDto)
    {
        try
        {
            logger.LogInformation($"ResetPassword for {resetPasswordDto.Id}");

            var user = await signInManager.UserManager.FindByIdAsync(resetPasswordDto.Id);

            if (user == null)
            {
                logger.LogError($"User could not be found using Id: {resetPasswordDto.Id}");

                return new JsonResult(new { message = Translations.User.UserWithIdNotFound});
            }

            var resetToken = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

            string newPassword = PasswordGenerator.GenerateRandomPassword();

            var result = await signInManager.UserManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                return new JsonResult(new { message = Translations.User.ResetPasswordSuccess
                    .Replace("{newPassword}", newPassword)
                    .Replace("{email}", user.Email)});
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ResetPassword failed for {resetPasswordDto.Id}");
        }
        return new JsonResult(new { message = Translations.User.ResetPasswordFailed.Replace("{id}", resetPasswordDto.Id) });
    }
    
    [HttpPost]
    public async Task<UserListReponse> DeleteUser([FromBody]UserDto deleteUserDto)
    {
        try
        {
            logger.LogInformation($"DeleteUser for {deleteUserDto.Id}");

            var user = await signInManager.UserManager.FindByIdAsync(deleteUserDto.Id);

            if (user == null)
            {
                logger.LogError($"User could not be found using id: {deleteUserDto.Id}");

                return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
            }

            var currentUser = await signInManager.UserManager.GetUserAsync(User);
            if (currentUser != null && user.Id == currentUser.Id)
            {
                logger.LogError("Cannot delete currently logged in user");

                return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
            }

            await signInManager.UserManager.DeleteAsync(user);

            return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"DeleteUser failed for {deleteUserDto.Id}");
        }
        return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
    }

    public async Task<UserListReponse> Deactivate([FromBody] UserDto userDto)
    {
        try
        {
            logger.LogInformation($"Deactivate for {userDto.Id}");

            var user = await signInManager.UserManager.FindByIdAsync(userDto.Id);

            if (user == null)
            {
                logger.LogError($"User could not be found using id: {userDto.Id}");

                return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
            }

            var currentUser = await signInManager.UserManager.GetUserAsync(User);
            if (currentUser != null && user.Id == currentUser.Id)
            {
                logger.LogError("Cannot deactivate currently logged in user");

                return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
            }

            await signInManager.UserManager.SetLockoutEnabledAsync(user, true);
            await signInManager.UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            
            return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Deactivate failed for {userDto.Id}");
        }
        return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
    }
    
    public async Task<UserListReponse> Activate([FromBody] UserDto userDto)
    {
        try
        {
            logger.LogInformation($"Activate for {userDto.Id}");

            var user = await signInManager.UserManager.FindByIdAsync(userDto.Id);

            if (user == null)
            {
                logger.LogError($"User could not be found using id: {userDto.Id}");

                return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
            }

            var currentUser = await signInManager.UserManager.GetUserAsync(User);
            if (currentUser != null && user.Id == currentUser.Id)
            {
                logger.LogError("Cannot activate currently logged in user");

                return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
            }

            await signInManager.UserManager.SetLockoutEnabledAsync(user, false);
            await signInManager.UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(-1));
            
            return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Activate failed for {userDto.Id}");
        }
        return new UserListReponse { Users = signInManager.UserManager.Users.OrderBy(x => x.Email).ToList() };
    }
}