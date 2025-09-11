namespace Infrastructure.Identity.Abstracts;
public interface IAuthTokenProcessor
{
    (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(ApplicationUser user, IEnumerable<string>? roles = null);
    string GenerateRefreshToken();
    void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration, bool isPersistent);
}
