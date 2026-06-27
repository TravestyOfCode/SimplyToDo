using Microsoft.AspNetCore.Authentication;

namespace SimplyToDo.Web.Models.Account;

public class RegisterVM
{
    public RegisterModel? Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = [];

    public string? ReturnUrl { get; set; }
}
