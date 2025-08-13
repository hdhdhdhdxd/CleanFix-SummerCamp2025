using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Materials.Commands.UpdateMaterial;

public record UpdateMaterialCommand : IRequest<int>
{
    public UpdateMaterialDto Material { get; init; }
}

public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, int>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public UpdateMaterialCommandHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = _mapper.Map<Material>(request.Material);
        await _materialRepository.UpdateAsync(material, cancellationToken);
        return material.Id;
    }
}
