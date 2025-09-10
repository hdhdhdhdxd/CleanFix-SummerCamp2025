using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password, bool RememberMe) : IRequest;

public class LoginCommandHandler : IRequestHandler<LoginCommand>
{
    private readonly IIdentityService _identityService;
    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.LoginAsync(request.Email, request.Password, request.RememberMe);

        if (!result.Succeeded)
        {
            throw new LoginFailedException(request.Email!);
        }
    }
}


