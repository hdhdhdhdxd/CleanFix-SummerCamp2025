using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Materials.Queries.GetMaterial;

public record GetMaterialQuery(int Id) : IRequest<GetMaterialDto>;

public class GetMaterialQueryHandler : IRequestHandler<GetMaterialQuery, GetMaterialDto>
{
    private readonly IMaterialRepository materialRepository;
    private readonly IMapper mapper;

    public GetMaterialQueryHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        this.materialRepository = materialRepository;
        this.mapper = mapper;
    }

    public async Task<GetMaterialDto> Handle(GetMaterialQuery request, CancellationToken cancellationToken)
    {
        var material = await materialRepository.GetByIdAsync(request.Id, cancellationToken);
        if (material == null)
            return null;
        var result = mapper.Map<GetMaterialDto>(material);
        return result;
    }
}
