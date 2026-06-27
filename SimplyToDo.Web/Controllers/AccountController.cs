using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplyToDo.Data.Services;
using SimplyToDo.Web.Models.Account;

namespace SimplyToDo.Web.Controllers;

public class AccountController(IAccountManager _accountManager, ILogger<AccountController> _logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie (per MS scafolded login page)
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        List<AuthenticationScheme> externalLogins = [.. await _accountManager.GetExternalAuthenticationSchemesAsync()];

        return View(new LoginVM()
        {
            ExternalLogins = externalLogins,
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel input, string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var result = await _accountManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with email {email}.", input.Email);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User attempted to login on locked out account using email {email}.", input.Email);
                ModelState.TryAddModelError(string.Empty, "The account has been locked out. Pleased contact your administrator.");
            }
            else
            {
                ModelState.TryAddModelError(string.Empty, "Invalid login attemp.");
            }
        }

        // There was a problem, return view with errors in ModelState.
        return View(new LoginVM()
        {
            ExternalLogins = [.. await _accountManager.GetExternalAuthenticationSchemesAsync()],
            ReturnUrl = returnUrl,
            Input = input
        });
    }

    [HttpGet]
    public IActionResult Logout()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");

        await _accountManager.SignOutAsync();

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    public async Task<IActionResult> Register(string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");
        List<AuthenticationScheme> externalLogins = [.. await _accountManager.GetExternalAuthenticationSchemesAsync()];

        return View(new RegisterVM()
        {
            ExternalLogins = externalLogins,
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel input, string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var result = await _accountManager.RegisterUserAsync(input.Email, input.Password);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(string.Empty, error.Description);
            }
        }

        // There was some errors, return view with ModelState errors.
        return View(new RegisterVM()
        {
            ExternalLogins = [.. await _accountManager.GetExternalAuthenticationSchemesAsync()],
            ReturnUrl = returnUrl,
            Input = input
        });
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string code, string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");

        if (userId == null || code == null)
        {
            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        var result = await _accountManager.ConfirmEmail(userId, code);

        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }

        return View();
    }
}
