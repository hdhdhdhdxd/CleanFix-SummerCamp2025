using Application.Users.Commands.Login;
using Application.Users.Commands.Refresh;
using Application.Users.Commands.Register;
using Infrastructure.Identity.Constants;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        await _sender.Send(query);

        return TypedResults.NoContent();
    }

    [HttpPost]
    [Route("register")]
    public async Task<Created> Register([FromBody] RegisterCommand query)
    {
        await _sender.Send(query);

        return TypedResults.Created();
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<Results<NoContent, BadRequest<string>>> Refresh([FromQuery]bool rememberMe)
    {
        var refreshToken = HttpContext.Request.Cookies[AuthCookieNames.RefreshToken];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return TypedResults.BadRequest("Refresh token is missing in cookies.");
        }

        await _sender.Send(new RefreshCommand(refreshToken, rememberMe));
        return TypedResults.NoContent();
    }
}
