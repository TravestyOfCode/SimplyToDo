using Microsoft.AspNetCore.Authentication;

namespace SimplyToDo.Web.Models.Account;

public class LoginVM
{
    public LoginModel? Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = [];

    public string? ReturnUrl { get; set; }
}
