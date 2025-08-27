using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Common.Mappings;
using Infrastructure.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetPaginatedCompanies;
public record GetPaginatedCompaniesQuery(int PageNumber, int PageSize, int? TypeIssueId) : IRequest<PaginatedList<GetPaginatedCompanyDto>>;

public class GetPaginatedCompaniesQueryHandler : IRequestHandler<GetPaginatedCompaniesQuery, PaginatedList<GetPaginatedCompanyDto>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetPaginatedCompaniesQueryHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedCompanyDto>> Handle(GetPaginatedCompaniesQuery request, CancellationToken cancellationToken)
    {
        var query = _companyRepository.GetQueryable().AsNoTracking();

        if (request.TypeIssueId != null)
        {
            query = query.Where(c => c.IssueTypeId == request.TypeIssueId);
        }

        var companies = await query
            .Include(c => c.IssueType)
            .ProjectTo<GetPaginatedCompanyDto>(_mapper.ConfigurationProvider).PaginatedListAsync(request.PageNumber, request.PageSize);


        return companies;
    }
}
