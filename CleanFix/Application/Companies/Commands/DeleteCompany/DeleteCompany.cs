using Application.Common.Interfaces;
using MediatR;

namespace Application.Companies.Commands.DeleteCompany;

public record DeleteCompanyCommand(int Id) : IRequest<bool>;

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCompanyCommandHandler(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id);

        if (company == null)
            return false;

        _companyRepository.Remove(company);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
