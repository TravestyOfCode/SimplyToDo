using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SimplyToDo.Data.Entities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace SimplyToDo.Data.Services;

internal class AccountManager(SignInManager<AppUser> _signInManager, ILogger<AccountManager> _logger) : IAccountManager
{
    #region // INTERFACE FUNCTIONS ////////////////////////////////////////////////////
    public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync() => await _signInManager.GetExternalAuthenticationSchemesAsync();

    public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe)
    {
        return await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: true);
    }

    public async Task<IdentityResult> RegisterUserAsync(string email, string password)
    {
        var user = new AppUser();

        await _signInManager.UserManager.SetUserNameAsync(user, email);
        await _signInManager.UserManager.SetEmailAsync(user, email);
        var result = await _signInManager.UserManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User create new account with email {email}.", email);

            SendAccountConfirmationEmailAsync(user);
        }

        return result;
    }

    public async Task SignOutAsync() => await _signInManager.SignOutAsync();

    public async Task<IdentityResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return IdentityResult.Failed();
        }

        var user = await _signInManager.UserManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed();
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        return await _signInManager.UserManager.ConfirmEmailAsync(user, code);
    }

    public bool IsSignedIn(ClaimsPrincipal principal) => _signInManager.IsSignedIn(principal);
    #endregion

    #region // PRIVATE FUNCTIONS //////////////////////////////////////////////////////
    private async void SendAccountConfirmationEmailAsync(AppUser user)
    {
        var userId = await _signInManager.UserManager.GetUserIdAsync(user);
        var code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // TODO: Best way to 'configure' the callback URL for registering? I don't think the Data project
        // should know about this and it should be configured when adding data services?
        var callbackUrl = $"/Account/ConfirmEmail?userId={userId}&code={code}";

        // TODO: Configure to use an actual email sender to send emails. For now, we'll just display in the
        // console.
        //await _emailSender.SendEmailAsync(user.Email!, "Confirm your email",
        //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        _logger.LogInformation("Please confirm your account using the following link {callback}", HtmlEncoder.Default.Encode(callbackUrl));
    }

    #endregion
}