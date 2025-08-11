using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Materials.Commands.CreateMaterial;

public record CreateMaterialCommand : IRequest<Guid>
{
    public CreateMaterialDto Material { get; init; }
}

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, Guid>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public CreateMaterialCommandHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = _mapper.Map<Material>(request.Material);

        if (material.Id == Guid.Empty)
            material.Id = Guid.NewGuid();

        var result = await _materialRepository.AddAsync(material, cancellationToken);

        return result;
    }
}
