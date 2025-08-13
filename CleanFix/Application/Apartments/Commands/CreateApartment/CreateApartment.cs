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
    private readonly IMapper _mapper;

    public CreateApartmentCommandHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = _mapper.Map<Apartment>(request.Apartment);

        if (apartment.Id == 0)
            apartment.Id = 0; // El Id será autoincremental en la base de datos

        var result = await _apartmentRepository.AddAsync(apartment, cancellationToken);

        return result;
    }
}
