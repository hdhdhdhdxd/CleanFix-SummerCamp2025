using Application.Common.Interfaces;
using MediatR;

namespace Application.Apartments.Commands.DeleteApartment;

public record DeleteApartmentCommand(Guid Id) : IRequest<bool>;

public class DeleteApartmentCommandHandler : IRequestHandler<DeleteApartmentCommand, bool>
{
    private readonly IApartmentRepository _apartmentRepository;

    public DeleteApartmentCommandHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<bool> Handle(DeleteApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (apartment == null)
            return false;

        await _apartmentRepository.RemoveAsync(apartment, cancellationToken);

        return true;
    }
}
