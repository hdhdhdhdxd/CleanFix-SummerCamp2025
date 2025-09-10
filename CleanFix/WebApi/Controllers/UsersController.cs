using Application.Users.Commands.Login;
using Application.Users.Commands.Refresh;
using Application.Users.Commands.Register;
using Infrastructure.Identity.Constants;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
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
    public async Task<Created> Register([FromBody] RegisterCommand query)
    {
        Log.Information("POST api/users/register called for user {User}", query.Email);
        await _sender.Send(query);
        Log.Information("User {User} registered successfully.", query.Email);
        return TypedResults.Created();
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<Results<NoContent, BadRequest<string>>> Refresh([FromQuery]bool rememberMe)
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
}
