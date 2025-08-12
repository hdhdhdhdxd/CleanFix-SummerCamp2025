using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Commands.CreateRequest;

public record CreateRequestCommand : IRequest<Guid>
{
    public CreateRequestDto Request { get; init; }
}
public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Guid>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public CreateRequestCommandHandler(IRequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Request>(request.Request);
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();
        var result = await _requestRepository.AddAsync(entity, cancellationToken);
        return result;
    }
}
