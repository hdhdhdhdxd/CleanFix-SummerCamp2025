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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMaterialCommandHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = _mapper.Map<Material>(request.Material);
        
        _materialRepository.Update(material);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return material.Id;
    }
}
