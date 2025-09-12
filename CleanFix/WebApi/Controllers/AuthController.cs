using Application.Auth.Commands.Login;
using Application.Auth.Commands.Refresh;
using Application.Auth.Commands.Register;
using Application.Auth.Queries.Me;
using Application.Common.Interfaces;
using Application.Common.Security;
using Infrastructure.Identity.Constants;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IUser _currentUser;

    public AuthController(ISender sender, IUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    [HttpPost]
    [Route("login")]
    public async Task<Results<Ok, NoContent>> Login([FromBody] LoginCommand query)
    {
        Log.Information("POST api/users/login called for user {User}", query.Email);
        await _sender.Send(query);
        Log.Information("User {User} logged in successfully.", query.Email);
        return TypedResults.NoContent();
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] RegisterCommand query)
    {
        if (!ModelState.IsValid)
        {
            Log.Warning("POST api/users/register failed validation for user {User}. Errors: {Errors}", query.Email, string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }
        Log.Information("POST api/users/register called for user {User}", query.Email);
        await _sender.Send(query);
        Log.Information("User {User} registered successfully.", query.Email);
        return Created();
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<Results<NoContent, BadRequest<string>>> Refresh([FromQuery] bool rememberMe = false)
    {
        var refreshToken = HttpContext.Request.Cookies[AuthCookieNames.RefreshToken];
        Log.Information("POST api/users/refresh called. RememberMe={RememberMe}", rememberMe);
        if (string.IsNullOrEmpty(refreshToken))
        {
            Log.Warning("Refresh token is missing in cookies.");
            return TypedResults.BadRequest("Refresh token is missing in cookies.");
        }
        await _sender.Send(new RefreshCommand(refreshToken, rememberMe));
        Log.Information("Refresh token processed successfully.");
        return TypedResults.NoContent();
    }

    [Authorize]
    [HttpGet]
    [Route("me")]
    public async Task<Results<Ok<UserInfoDto>, NoContent>> Me()
    {
        var userId = _currentUser.Id;

        Log.Information("GET api/auth/me called for userId {UserId}.", userId);

        var result = await _sender.Send(new MeQuery(userId!.Value));

        Log.Information("Authenticated user found: {User}", result.Email);
        return TypedResults.Ok(result);
    }

    [HttpGet]
    [Route("isAuthenticated")]
    public ActionResult IsAuthenticated()
    {
        var isAuthenticated = _currentUser.Id.HasValue;

        Log.Information("GET api/auth/isAuthenticated called. IsAuthenticated={IsAuthenticated}", isAuthenticated);

        return Ok(isAuthenticated);
    }

    [HttpPost]
    [Route("logout")]
    public IActionResult Logout()
    {
        // Eliminar las cookies ACCESS_TOKEN y REFRESH_TOKEN
        Response.Cookies.Append(AuthCookieNames.AccessToken, "", new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true
        });
        Response.Cookies.Append(AuthCookieNames.RefreshToken, "", new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            IsEssential = true,
            Path = "/api/auth/refresh"
        });
        Log.Information("POST api/auth/logout called. Cookies ACCESS_TOKEN and REFRESH_TOKEN removed.");
        return NoContent();
    }
}
