using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;

namespace SimplyToDo.Data.Services;

public interface IAccountManager
{
    /// <summary>
    /// Gets a collection of <see cref="AuthenticationScheme"/>s for the know external login providers."
    /// </summary>
    /// <returns>A collection of <see cref="AuthenticationScheme"/>s for the know external login providers.</returns>
    public Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();

    /// <summary>
    /// Attempts to sign in the specified account with the supplied email and password combination.
    /// </summary>
    /// <param name="email">The email of the account to sign in.</param>
    /// <param name="password">The password to attempt to sign in with.</param>
    /// <param name="rememberMe">Flag indicating wither the sign-in cookie should persist after browser is closed.</param>
    /// <returns></returns>
    public Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe);

    /// <summary>
    /// Signs the current user out of the application.
    /// </summary>
    /// <returns><see cref="Task"/></returns>
    public Task SignOutAsync();

    /// <summary>
    /// Attempts to register a new account with the supplied email and password combination.
    /// </summary>
    /// <param name="email">The email of the account to register.</param>
    /// <param name="password">The password for the account.</param>
    /// <returns>An <see cref="IdentityResult"/> that indicates if the registration was successful.</returns>
    public Task<IdentityResult> RegisterUserAsync(string email, string password);

    /// <summary>
    /// Confirms if the registration code sent to the account's email is valid.
    /// </summary>
    /// <param name="userId">The user who's email was sent the code.</param>
    /// <param name="code">The code that was sent to the account's email address.</param>
    /// <returns>An <see cref="IdentityResult"/> that indicates if the sent code matches the provided code.</returns>
    public Task<IdentityResult> ConfirmEmail(string userId, string code);
}

internal class AccountManager(SignInManager<AppUser> _signInManager, IEmailSender _emailSender, ILogger<AccountManager> _logger) : IAccountManager
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
    #endregion

    #region // PRIVATE FUNCTIONS //////////////////////////////////////////////////////
    private async void SendAccountConfirmationEmailAsync(AppUser user)
    {
        var userId = await _signInManager.UserManager.GetUserIdAsync(user);
        var code = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // TODO: Best way to 'configure' the callback URL for registering? I don't think the Data project
        // should know about this and it should be configured when adding data services?
        var callbackUrl = $"/Identity/Account/ConfirmEmail&userId={userId}&code={code}";

        await _emailSender.SendEmailAsync(user.Email!, "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
    }

    #endregion
}