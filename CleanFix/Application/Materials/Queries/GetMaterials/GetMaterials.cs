using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Materials.Queries.GetMaterials;

public record GetMaterialsQuery : IRequest<List<GetMaterialsDto>>;

public class GetMaterialsQueryHandler : IRequestHandler<GetMaterialsQuery, List<GetMaterialsDto>>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public GetMaterialsQueryHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<List<GetMaterialsDto>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
    {
        var materials = await _materialRepository.GetAll().ToListAsync(cancellationToken);
        var result = _mapper.Map<List<GetMaterialsDto>>(materials);
        return result;
    }
}
