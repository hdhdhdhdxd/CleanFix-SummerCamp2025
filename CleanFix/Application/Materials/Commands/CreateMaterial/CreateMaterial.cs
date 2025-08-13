using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Materials.Commands.CreateMaterial;

public record CreateMaterialCommand : IRequest<int>
{
    public CreateMaterialDto Material { get; init; }
}

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, int>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public CreateMaterialCommandHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = _mapper.Map<Material>(request.Material);
        if (material.Id == 0)
            material.Id = 0; // El Id será autoincremental en la base de datos
        var result = await _materialRepository.AddAsync(material, cancellationToken);
        return result;
    }
}
