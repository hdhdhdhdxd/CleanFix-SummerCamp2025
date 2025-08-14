using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Companies.Queries.GetCompany;
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
        var company = await _companyRepository.GetByIdAsync(request.Id);
        if (company == null)
            return null;
        var result = _mapper.Map<GetCompanyDto>(company);
        return result;
    }
}
