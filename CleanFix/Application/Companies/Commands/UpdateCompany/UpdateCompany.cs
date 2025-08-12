using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand : IRequest
{
    public UpdateCompanyDto Company { get; init; }
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(request.Company);
        await _companyRepository.UpdateAsync(company, cancellationToken);
    }
}
