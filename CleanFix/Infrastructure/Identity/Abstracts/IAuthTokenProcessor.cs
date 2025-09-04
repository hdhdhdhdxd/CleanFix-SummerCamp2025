namespace Infrastructure.Identity.Abstracts;
public interface IAuthTokenProcessor
{
    (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(ApplicationUser user);
    string GenerateRefreshToken();
    void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
}
