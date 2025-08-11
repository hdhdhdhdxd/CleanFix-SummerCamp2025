using AutoMapper;
using Domain.Entities;
using MediatR;
using WebApi.Interfaces;

namespace Application.Materials.Commands.UpdateMaterial;

public record UpdateMaterialCommand : IRequest
{
    public UpdateMaterialDto Material { get; init; }
}

public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public UpdateMaterialCommandHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = _mapper.Map<Material>(request.Material);
        await _materialRepository.UpdateAsync(material, cancellationToken);
    }
}
