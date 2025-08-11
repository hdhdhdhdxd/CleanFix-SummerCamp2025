using AutoMapper;
using Domain.Entities;
using MediatR;
using WebApi.Interfaces;

namespace Application.Companies.Commands.CreateCompany;

public record CreateCompanyCommand : IRequest<Guid>
{
    public CreateCompanyDto Company { get; init; }
}
public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public CreateCompanyCommandHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(request.Company);

        if (company.Id == Guid.Empty)
            company.Id = Guid.NewGuid();

        var result = await _companyRepository.AddAsync(company, cancellationToken);

        return result;
    }
}
