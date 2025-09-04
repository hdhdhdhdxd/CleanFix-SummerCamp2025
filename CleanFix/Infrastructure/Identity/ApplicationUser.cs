using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;
public class ApplicationUser : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAtUtc { get; set; }

    public static ApplicationUser Create(string email)
    {
        return new ApplicationUser
        {
            Email = email,
            UserName = email,
        };
    }
}
