using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Commands.UpdateRequest;

public record UpdateRequestCommand : IRequest
{
    public UpdateRequestDto Request { get; init; }
}

public class UpdateRequestCommandHandler : IRequestHandler<UpdateRequestCommand>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IMapper _mapper;

    public UpdateRequestCommandHandler(IRequestRepository requestRepository, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Request>(request.Request);
        await _requestRepository.UpdateAsync(entity, cancellationToken);
    }
}
