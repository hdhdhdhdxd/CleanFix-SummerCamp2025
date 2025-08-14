using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Apartments.Commands.CreateApartment;

public record CreateApartmentCommand : IRequest<int>
{
    public CreateApartmentDto Apartment { get; init; }
}
public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, int>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateApartmentCommandHandler(IApartmentRepository apartmentRepository,IUnitOfWork unitOfWork, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = _mapper.Map<Apartment>(request.Apartment);

        var result = _apartmentRepository.Add(apartment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
