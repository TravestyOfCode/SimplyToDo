using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

    /// <summary>
    /// Returns true if the principal has an identity with the application cookie identity.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
    /// <returns>True if the user is logged in with identity.</returns>
    public bool IsSignedIn(ClaimsPrincipal principal);
}
