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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Request>(request.Request);
        if (entity.Id == 0)
            entity.Id = 0;
            
        _requestRepository.Add(entity);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.Id;
    }
}
