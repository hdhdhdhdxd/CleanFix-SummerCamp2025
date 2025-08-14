using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Solicitations.Queries.GetSolicitation;
public record GetSolicitationQuery(int Id) : IRequest<GetSolicitationDto>;

public class GetSolicitationQueryHandler : IRequestHandler<GetSolicitationQuery, GetSolicitationDto>
{
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IMapper _mapper;

    public GetSolicitationQueryHandler(ISolicitationRepository solicitationRepository, IMapper mapper)
    {
        _solicitationRepository = solicitationRepository;
        _mapper = mapper;
    }

    public async Task<GetSolicitationDto> Handle(GetSolicitationQuery request, CancellationToken cancellationToken)
    {
        var solicitation = await _solicitationRepository.GetByIdAsync(request.Id);
        var result = _mapper.Map<GetSolicitationDto>(solicitation);
        return result;
    }
}
