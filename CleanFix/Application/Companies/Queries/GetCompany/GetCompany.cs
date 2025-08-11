using AutoMapper;
using MediatR;
using WebApi.Interfaces;

namespace Application.Companies.Queries.GetCompany;
public record GetCompanyQuery(Guid Id) : IRequest<GetCompanyDto>;

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
        var company = await _companyRepository.GetByIdAsync(request.Id, cancellationToken);
        var result = _mapper.Map<GetCompanyDto>(company);
        return result;
    }
}
