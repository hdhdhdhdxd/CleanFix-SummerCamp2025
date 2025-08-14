using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Requests.Queries.GetRequest;
public record GetRequestQuery(int Id) : IRequest<GetRequestDto>;

public class GetRequestQueryHandler : IRequestHandler<GetRequestQuery, GetRequestDto>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public GetRequestQueryHandler(IRequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<GetRequestDto> Handle(GetRequestQuery request, CancellationToken cancellationToken)
    {
        var entity = await _requestRepository.GetByIdAsync(request.Id);
        var result = _mapper.Map<GetRequestDto>(entity);
        return result;
    }
}
