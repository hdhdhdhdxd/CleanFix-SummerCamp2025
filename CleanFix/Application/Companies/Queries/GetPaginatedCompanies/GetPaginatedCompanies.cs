using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Companies.Queries.GetPaginatedCompanies;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Common.Mappings;
using Infrastructure.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetPaginatedCompanies;
public record GetPaginatedCompaniesQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetPaginatedCompanyDto>>;

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
        var companies = await _companyRepository.GetAll()
            .AsNoTracking()
            .ProjectTo<GetPaginatedCompanyDto>(_mapper.ConfigurationProvider).PaginatedListAsync(request.PageNumber, request.PageSize);
       

        return companies;
    }
}
