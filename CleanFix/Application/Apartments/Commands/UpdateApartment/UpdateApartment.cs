using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Apartments.Commands.UpdateApartment;

public record UpdateApartmentCommand : IRequest
{
    public UpdateApartmentDto Apartment { get; init; }
}

public class UpdateApartmentCommandHandler : IRequestHandler<UpdateApartmentCommand>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateApartmentCommandHandler(IApartmentRepository apartmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = _mapper.Map<Apartment>(request.Apartment);

        _apartmentRepository.Update(apartment);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
