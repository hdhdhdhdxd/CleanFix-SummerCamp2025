using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Common.Mappings;
using Infrastructure.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Materials.Queries.GetPaginatedMaterials;
public record GetPaginatedMaterialsQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetPaginatedMaterialDto>>;

public class GetPaginatedMaterialsQueryHandler : IRequestHandler<GetPaginatedMaterialsQuery, PaginatedList<GetPaginatedMaterialDto>>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public GetPaginatedMaterialsQueryHandler(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedMaterialDto>> Handle(GetPaginatedMaterialsQuery request, CancellationToken cancellationToken)
    {
        var materials = await _materialRepository.GetAll()
            .AsNoTracking()
            .ProjectTo<GetPaginatedMaterialDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
             

            return materials;
    }
}
