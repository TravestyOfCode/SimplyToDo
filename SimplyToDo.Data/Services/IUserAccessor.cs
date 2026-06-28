using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SimplyToDo.Data.Services;

internal interface IUserAccessor
{
    public string? GetCurrentUser();
}

internal class HttpContextUserAccessor(IHttpContextAccessor _accessor) : IUserAccessor
{
    public string? GetCurrentUser() => _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
