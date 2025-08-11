using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Apartments.Commands.CreateApartment;

public record CreateApartmentCommand : IRequest<Guid>
{
    public CreateApartmentDto Apartment { get; init; }
}
public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, Guid>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public CreateApartmentCommandHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = _mapper.Map<Apartment>(request.Apartment);

        if (apartment.Id == Guid.Empty)
            apartment.Id = Guid.NewGuid();

        var result = await _apartmentRepository.AddAsync(apartment, cancellationToken);

        return result;
    }
}
