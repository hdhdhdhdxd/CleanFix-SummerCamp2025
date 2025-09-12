using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Companies.Queries.GetCompany;

[Authorize(Roles = Roles.Administrator)]
public record GetCompanyQuery(int Id) : IRequest<GetCompanyDto>;

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, GetCompanyDto>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetCompanyQueryHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<GetCompanyDto> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var query = _companyRepository.GetQueryable();

        var company = await query.Include(c => c.IssueType).FirstOrDefaultAsync(c => c.Id == request.Id);

        if (company == null)
            return null;

        var companyDto = _mapper.Map<GetCompanyDto>(company);

        return companyDto;
    }
}
