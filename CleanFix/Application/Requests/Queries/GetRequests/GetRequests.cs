using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Queries.GetRequests;
public record GetRequestsQuery : IRequest<List<GetRequestsDto>>;

public class GetRequestsQueryHandler : IRequestHandler<GetRequestsQuery, List<GetRequestsDto>>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public GetRequestsQueryHandler(IRequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<List<GetRequestsDto>> Handle(GetRequestsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _requestRepository.GetAll()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var result = _mapper.Map<List<GetRequestsDto>>(entities);
        return result;
    }
}
