using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
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
}
