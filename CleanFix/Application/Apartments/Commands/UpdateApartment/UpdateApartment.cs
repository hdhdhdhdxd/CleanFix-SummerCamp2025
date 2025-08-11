using AutoMapper;
using Domain.Entities;
using MediatR;
using WebApi.Interfaces;

namespace Application.Apartments.Commands.UpdateApartment;

public record UpdateApartmentCommand : IRequest
{
    public UpdateApartmentDto Apartment { get; init; }
}

public class UpdateApartmentCommandHandler : IRequestHandler<UpdateApartmentCommand>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public UpdateApartmentCommandHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = _mapper.Map<Apartment>(request.Apartment);

        await _apartmentRepository.UpdateAsync(apartment, cancellationToken);
    }
}
