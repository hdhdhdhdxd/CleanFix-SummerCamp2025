using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Identity.Abstracts;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthTokenProcessor _authTokenProcessor;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IAuthTokenProcessor authTokenProcessor)
    {
        _userManager = userManager;
        _authTokenProcessor = authTokenProcessor;
    }

    public async Task<Result> RegisterAsync(string email, string password)
    {
        var user = ApplicationUser.Create(email);
        var result = await _userManager.CreateAsync(user, password);

        return result.ToApplicationResult();
    }

    public async Task<Result> LoginAsync(string email, string password)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = await _userManager.FindByNameAsync(email);
        }

        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return Result.Failure(new[] { $"Invalid login attempt for email: {email}" });
        }

        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return updateResult.ToApplicationResult();
        }

        // Solo configurar cookies - no retornar tokens
        _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie(AuthCookieNames.AccessToken, jwtToken, expirationDateInUtc);
        _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie(AuthCookieNames.RefreshToken, user.RefreshToken, refreshTokenExpirationDateInUtc);

        return Result.Success();
    }

    public async Task<Result> RefreshTokenAsync(string? refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Result.Failure(["Refresh token is missing."]);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

        if (user == null)
        {
            return Result.Failure(["Unable to retrieve user for refresh token"]);
        }

        if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
        {
            return Result.Failure(["Refresh token is expired."]);
        }

        var (jwtToken, expirationDateInUtc) = _authTokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = _authTokenProcessor.GenerateRefreshToken();

        var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return updateResult.ToApplicationResult();
        }

        _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie(AuthCookieNames.AccessToken, jwtToken, expirationDateInUtc);
        _authTokenProcessor.WriteAuthTokenAsHttpOnlyCookie(AuthCookieNames.RefreshToken, user.RefreshToken, refreshTokenExpirationDateInUtc);

        return Result.Success();
    }

    public async Task<string?> GetUserNameAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return user?.UserName;
    }

    public async Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<Result> DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
}
