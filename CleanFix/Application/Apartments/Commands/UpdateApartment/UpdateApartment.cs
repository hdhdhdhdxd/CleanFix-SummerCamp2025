using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        try
        {
            _apartmentRepository.Update(apartment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("Conflicto de concurrencia: el registro fue modificado por otro usuario. Por favor, recargue y vuelva a intentar.");
        }
    }
}
