using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Common.Mappings;
using Infrastructure.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Queries.GetPaginatedRequests;
public record GetPaginatedRequestsQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetPaginatedRequestDto>>;

public class GetPaginatedRequestsQueryHandler : IRequestHandler<GetPaginatedRequestsQuery, PaginatedList<GetPaginatedRequestDto>>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public GetPaginatedRequestsQueryHandler(IRequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedRequestDto>> Handle(GetPaginatedRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _requestRepository.GetAll()
            .AsNoTracking()
            .ProjectTo<GetPaginatedRequestDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return requests;
    }
}
