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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMaterialCommandHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = _mapper.Map<Material>(request.Material);
        if (material.Id == 0)
            material.Id = 0; // El Id será autoincremental en la base de datos
            
        _materialRepository.Add(material);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return material.Id;
    }
}
