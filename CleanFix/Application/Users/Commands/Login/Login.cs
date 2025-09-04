using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Users.Commands.Login;

public record LoginCommand : IRequest
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand>
{
    private readonly IIdentityService _identityService;
    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        Guard.Against.NullOrEmpty(request.Password, nameof(request.Password));

        var result = await _identityService.LoginAsync(request.Email, request.Password);
        
        if (!result.Succeeded)
        {
            throw new LoginFailedException(request.Email!);
        }
    }
}


