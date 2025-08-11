using AutoMapper;
using MediatR;
using WebApi.Interfaces;

namespace Application.Materials.Queries.GetMaterial;

public record GetMaterialQuery(Guid Id) : IRequest<GetMaterialDto>;

public class GetMaterialQueryHandler : IRequestHandler<GetMaterialQuery, GetMaterialDto>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public GetMaterialQueryHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<GetMaterialDto> Handle(GetMaterialQuery request, CancellationToken cancellationToken)
    {
        var material = await _materialRepository.GetByIdAsync(request.Id, cancellationToken);
        if (material == null)
            return null;
        var result = _mapper.Map<GetMaterialDto>(material);
        return result;
    }
}
