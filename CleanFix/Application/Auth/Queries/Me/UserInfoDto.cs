namespace Application.Auth.Queries.Me;

public class UserInfoDto
{    public string? Email { get; set; }
    public string? Username { get; set; }
    public List<string> Roles { get; set; } = new();
}
