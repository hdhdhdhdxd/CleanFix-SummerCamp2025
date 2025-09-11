using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;


namespace Application.Auth.Queries.Me;
public record MeQuery(Guid UserId) : IRequest<UserInfoDto>;

public class MeQueryHandler : IRequestHandler<MeQuery, UserInfoDto>
{
    private readonly IIdentityService _identityService;

    public MeQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<UserInfoDto> Handle(MeQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.MeAsync(request.UserId);

        Guard.Against.NotFound(request.UserId, user);

        return user;
    }
}
