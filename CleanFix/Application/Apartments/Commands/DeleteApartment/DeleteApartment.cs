using MediatR;
using WebApi.Interfaces;

namespace Application.Apartments.Commands.DeleteApartment;

public record DeleteApartmentsCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class DeleteApartmentsCommandHandler
{
    private readonly IApartmentRepository _apartmentRepository;

    public DeleteApartmentsCommandHandler(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<bool> Handle(DeleteApartmentsCommand request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (apartment == null)
            return false;

        await _apartmentRepository.RemoveAsync(apartment, cancellationToken);

        return true;
    }
}
