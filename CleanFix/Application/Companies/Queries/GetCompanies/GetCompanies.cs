using MediatR;
using WebApi.Interfaces;
using AutoMapper;

namespace Application.Companies.Queries.GetCompanies;

public record GetCompaniesQuery : IRequest<List<GetCompaniesDto>>;

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<GetCompaniesDto>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public GetCompaniesQueryHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<List<GetCompaniesDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _companyRepository.GetAllAsync(cancellationToken);
        var result = _mapper.Map<List<GetCompaniesDto>>(companies);
        return result;
    }
}
