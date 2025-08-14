using Application.Common.Interfaces;
using MediatR;

namespace Application.Apartments.Commands.DeleteApartment;

public record DeleteApartmentCommand(int Id) : IRequest<bool>;

public class DeleteApartmentCommandHandler : IRequestHandler<DeleteApartmentCommand, bool>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteApartmentCommandHandler(IApartmentRepository apartmentRepository, IUnitOfWork unitOfWork)
    {
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteApartmentCommand request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(request.Id);

        if (apartment == null)
            return false;

        _apartmentRepository.Remove(apartment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
