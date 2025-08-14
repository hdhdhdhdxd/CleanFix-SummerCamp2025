using Application.Common.Interfaces;
using MediatR;

namespace Application.Materials.Commands.DeleteMaterial;

public record DeleteMaterialCommand(int Id) : IRequest<bool>;

public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, bool>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMaterialCommandHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
    {
        _materialRepository = materialRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _materialRepository.GetByIdAsync(request.Id);

        if (material == null)
            return false;

        _materialRepository.Add(material);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
