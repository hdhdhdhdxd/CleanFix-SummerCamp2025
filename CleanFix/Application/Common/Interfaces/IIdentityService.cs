using Application.Auth.Queries.Me;
using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result> LoginAsync(string email, string password, bool rememberMe);

    Task<Result> RegisterAsync(string email, string password);

    Task<Result> RefreshTokenAsync(string? refreshToken, bool rememberMe);

    Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string email, string password);

    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task<UserInfoDto?> MeAsync(Guid userId);
}
