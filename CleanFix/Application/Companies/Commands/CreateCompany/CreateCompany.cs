using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Companies.Commands.CreateCompany;

public record CreateCompanyCommand : IRequest<int>
{
    public CreateCompanyDto Company { get; init; }
}
public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, int>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(request.Company);
        if (company.Id == 0)
            company.Id = 0; // El Id será autoincremental en la base de datos
        
        var result = _companyRepository.Add(company);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return result;
    }
}
