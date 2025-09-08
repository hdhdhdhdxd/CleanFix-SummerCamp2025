using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Exceptions;
using MediatR;

namespace Application.Users.Commands.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand>
{
    private readonly IIdentityService _identityService;

    public RefreshCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        Result result = await _identityService.RefreshTokenAsync(request.RefreshToken);

        if (!result.Succeeded)
        {
            throw new RefreshTokenException(result.Errors);
        }
    }
}
