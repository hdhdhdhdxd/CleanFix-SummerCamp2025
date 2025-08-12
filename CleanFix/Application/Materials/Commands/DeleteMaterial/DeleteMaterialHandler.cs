using Application.Common.Interfaces;
using MediatR;

namespace Application.Materials.Commands.DeleteMaterial;

public record DeleteMaterialCommand(Guid Id) : IRequest<bool>;

public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, bool>
{
    private readonly IMaterialRepository _materialRepository;

    public DeleteMaterialCommandHandler(IMaterialRepository materialRepository)
    {
        _materialRepository = materialRepository;
    }

    public async Task<bool> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _materialRepository.GetByIdAsync(request.Id, cancellationToken);
        if (material == null) return false;
        await _materialRepository.RemoveAsync(material, cancellationToken);
        return true;
    }
}
