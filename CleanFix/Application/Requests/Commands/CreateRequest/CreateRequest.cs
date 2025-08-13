using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Commands.CreateRequest;

public record CreateRequestCommand : IRequest<int>
{
    public CreateRequestDto Request { get; init; }
}
public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, int>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public CreateRequestCommandHandler(IRequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Request>(request.Request);
        if (entity.Id == 0)
            entity.Id = 0;
        var result = await _requestRepository.AddAsync(entity, cancellationToken);
        return result;
    }
}
