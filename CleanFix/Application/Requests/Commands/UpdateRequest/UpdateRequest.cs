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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Request>(request.Request);
        
        _requestRepository.Update(entity);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
