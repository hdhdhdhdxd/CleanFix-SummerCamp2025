using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using MediatR;

namespace Application.CompletedTasks.Commands.CreateCompletedTask;

[Authorize(Roles = Roles.Administrator)]
public record CreateCompletedTaskCommand : IRequest<int>
{
    public CreateCompletedTaskDto CompletedTask { get; init; }
}

public class CreateCompletedTaskCommandHandler : IRequestHandler<CreateCompletedTaskCommand, int>
{
    private readonly ICompletedTaskRepository _completedTaskRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IExternalIncidenceRepository _externalIncidenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompletedTaskCommandHandler(
        ICompletedTaskRepository completedTaskRepository,
        IMaterialRepository materialRepository,
        ISolicitationRepository solicitationRepository,
        IIncidenceRepository incidenceRepository,
        ICompanyRepository companyRepository,
        IExternalIncidenceRepository externalIncidenceRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRequestRepository requestRepository)
    {
        _completedTaskRepository = completedTaskRepository;
        _materialRepository = materialRepository;
        _solicitationRepository = solicitationRepository;
        _incidenceRepository = incidenceRepository;
        _companyRepository = companyRepository;
        _requestRepository = requestRepository;
        _externalIncidenceRepository = externalIncidenceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCompletedTaskCommand request, CancellationToken cancellationToken)
    {
        if (request?.CompletedTask == null)
            throw new ArgumentNullException(nameof(request), "CompletedTask cannot be null");

        var completedTask = _mapper.Map<CompletedTask>(request.CompletedTask);
        completedTask.Materials = await GetMaterialsAsync(request.CompletedTask.MaterialIds);
        completedTask.CreationDate = DateTime.UtcNow;
        completedTask.CompletionDate = DateTime.UtcNow.AddDays(Random.Shared.Next(1, 31));

        var company = await GetCompanyAsync(request.CompletedTask.CompanyId);
        completedTask.Company = company;
        completedTask.CompanyId = company.Id;

        decimal total;
        if (request.CompletedTask.IsSolicitation && request.CompletedTask.SolicitationId.HasValue)
        {
            total = await HandleSolicitationAsync(request.CompletedTask, completedTask, company, cancellationToken);
        }
        else if (!request.CompletedTask.IsSolicitation && request.CompletedTask.IncidenceId.HasValue)
        {
            total = await HandleIncidenceAsync(request.CompletedTask, completedTask, company, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException("Debe especificar SolicitationId o IncidenceId correctamente.");
        }

        completedTask.Price = (double)total;
        completedTask.IsSolicitation = request.CompletedTask.IsSolicitation;

        _completedTaskRepository.Add(completedTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return completedTask.Id;
    }

    private async Task<List<Material>> GetMaterialsAsync(int[]? materialIds)
    {
        var ids = materialIds ?? Array.Empty<int>();
        var materials = new List<Material>();
        foreach (var id in ids)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null)
                throw new InvalidOperationException($"Material con ID {id} no existe.");
            materials.Add(material);
        }
        return materials;
    }

    private async Task<Company> GetCompanyAsync(int companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);
        if (company == null)
            throw new InvalidOperationException($"Company con ID {companyId} no existe.");
        return company;
    }

    private async Task<decimal> HandleSolicitationAsync(CreateCompletedTaskDto dto, CompletedTask completedTask, Company company, CancellationToken cancellationToken)
    {
        var solicitation = await _solicitationRepository.GetByIdAsync(dto.SolicitationId!.Value);
        if (solicitation == null)
            throw new InvalidOperationException($"Solicitation con ID {dto.SolicitationId.Value} no existe.");

        int apartmentCount = solicitation.ApartmentAmount;
        decimal total = company.Price * apartmentCount;
        foreach (var material in completedTask.Materials)
            total += material.Cost * apartmentCount;

        total += company.Price;

        completedTask.Address = solicitation.Address ?? string.Empty;
        completedTask.IssueTypeId = solicitation.IssueTypeId;
        completedTask.IssueType = solicitation.IssueType;

        // Use the injected RequestRepository to update the external request cost
        var result = await _requestRepository.UpdateRequestCost(solicitation.BuildingCode, total);
        
        if (!result.Succeeded)
        {
            throw new ExternalServiceException<string>(solicitation.BuildingCode, result.Errors);
        }

        _solicitationRepository.Remove(solicitation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return total;
    }

    private async Task<decimal> HandleIncidenceAsync(CreateCompletedTaskDto dto, CompletedTask completedTask, Company company, CancellationToken cancellationToken)
    {
        var incidence = await _incidenceRepository.GetByIdAsync(dto.IncidenceId!.Value);
        if (incidence == null)
            throw new InvalidOperationException($"Incidence con ID {dto.IncidenceId.Value} no existe.");

        decimal total = company.Price;
        foreach (var material in completedTask.Materials)
            total += incidence.Surface * material.CostPerSquareMeter;

        completedTask.IssueTypeId = incidence.IssueTypeId;
        completedTask.IssueType = incidence.IssueType;
        completedTask.Surface = incidence.Surface;
        completedTask.Address = incidence.Address;

        // Use the injected ExternalIncidenceRepository to update the external incidence 

        var result = await _externalIncidenceRepository.UpdateIncidenceCost(incidence.IncidenceId, company.Name);

        if (!result.Succeeded)
        {
            throw new ExternalServiceException<int>(incidence.IncidenceId, result.Errors);
        }

        _incidenceRepository.Remove(incidence);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return total;
    }
}
