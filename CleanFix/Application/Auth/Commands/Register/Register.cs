using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using RegistrationFailedException = Application.Common.Exceptions.RegistrationFailedException;
using System.ComponentModel.DataAnnotations;

namespace Application.Auth.Commands.Register;

public record RegisterCommand : IRequest
{
    [Required]
    [EmailAddress]
    public string? Email { get; init; }

    [Required]
    [MinLength(4)]
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

        var result = await _identityService.RegisterAsync(request.Email, request.Password);
        
        if (!result.Succeeded)
        {
            throw new RegistrationFailedException(result.Errors);
        }
    }
}
