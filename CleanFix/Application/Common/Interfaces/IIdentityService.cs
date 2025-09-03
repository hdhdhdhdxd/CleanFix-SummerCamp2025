using System.Security.Claims;
using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task LoginAsync(string email, string password);

    Task RegisterAsync(string email, string password);

    Task<string?> GetUserNameAsync(Guid userId);

    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(Guid userId);
}
