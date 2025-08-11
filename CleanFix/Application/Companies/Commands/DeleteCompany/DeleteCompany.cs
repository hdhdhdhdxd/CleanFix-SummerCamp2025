using Application.Common.Interfaces;
using MediatR;

namespace Application.Companies.Commands.DeleteCompany;

public record DeleteCompanyCommand(Guid Id) : IRequest<bool>;

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, bool>
{
    private readonly ICompanyRepository _companyRepository;

    public DeleteCompanyCommandHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (company == null)
            return false;
        await _companyRepository.RemoveAsync(company, cancellationToken);
        return true;
    }
}
