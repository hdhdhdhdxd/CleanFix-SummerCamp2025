
using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Users.Commands.Register;
public record RegisterCommand : IRequest
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.NullOrEmpty(request.Email, nameof(request.Email));
        Guard.Against.NullOrEmpty(request.Password, nameof(request.Password));

        await _identityService.RegisterAsync(request.Email, request.Password);
    }
}
