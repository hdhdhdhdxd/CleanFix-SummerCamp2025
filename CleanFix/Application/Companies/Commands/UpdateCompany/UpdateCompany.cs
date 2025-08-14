using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand : IRequest<int>
{
    public UpdateCompanyDto Company { get; init; }
}
public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, int>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(request.Company);
        
        _companyRepository.Update(company);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return company.Id;
    }
}
