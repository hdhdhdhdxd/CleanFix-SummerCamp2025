using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Interfaces;

namespace WebApi.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? Id
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            
            if (httpContext?.User == null)
            {
                return null;
            }

            var idString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(idString, out var guid) ? guid : null;
        }
    }
}
