using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CompletedTasks.Commands.CreateCompletedTask;

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
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompletedTaskCommandHandler(
        ICompletedTaskRepository completedTaskRepository,
        IMaterialRepository materialRepository,
        ISolicitationRepository solicitationRepository,
        IIncidenceRepository incidenceRepository,
        ICompanyRepository companyRepository,
        IApartmentRepository apartmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _completedTaskRepository = completedTaskRepository;
        _materialRepository = materialRepository;
        _solicitationRepository = solicitationRepository;
        _incidenceRepository = incidenceRepository;
        _companyRepository = companyRepository;
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCompletedTaskCommand request, CancellationToken cancellationToken)
    {
        var completedTask = _mapper.Map<CompletedTask>(request.CompletedTask);

        // Obtener materiales por IDs
        var materialIds = request.CompletedTask.MaterialIds;
        var materials = new List<Material>();
        foreach (var id in materialIds)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null)
                throw new Exception($"Material con ID {id} no existe.");
            materials.Add(material);
        }
        completedTask.Materials = materials;

        completedTask.CreationDate = DateTime.UtcNow;
        completedTask.CompletionDate = DateTime.UtcNow.AddDays(Random.Shared.Next(1, 31));

        // Calcular el total y llenar campos
        decimal total = 0;
        var company = await _companyRepository.GetByIdAsync(request.CompletedTask.ComapanyId);
        if (company == null)
            throw new Exception($"Company con ID {request.CompletedTask.ComapanyId} no existe.");
        completedTask.Company = company;
        completedTask.CompanyId = company.Id;

        string? simulatedPutResult = null;

        if (request.CompletedTask.IsSolicitation && request.CompletedTask.SolicitationId.HasValue)
        {
            var solicitation = await _solicitationRepository.GetByIdAsync(request.CompletedTask.SolicitationId.Value);
            if (solicitation == null)
                throw new Exception($"Solicitation con ID {request.CompletedTask.SolicitationId.Value} no existe.");
            int apartmentCount = solicitation.RequestId;
            total += company.Price * apartmentCount;
            foreach (var material in materials)
                total += material.Cost * apartmentCount;
            // Llenar campos desde Solicitation
            completedTask.Address = solicitation.Address;
            completedTask.IssueTypeId = solicitation.IssueTypeId;
            completedTask.IssueType = solicitation.IssueType;
            // Simulación de PUT a API externa para Solicitation (antes de guardar)
            var putData = new { IdRequest = solicitation.RequestId, Total = (double)total };
            simulatedPutResult = await SimulatePutToExternalApiAsync($"https://api.solicitations.com/solicitations/{solicitation.RequestId}", putData);
            if (simulatedPutResult != "OK")
                throw new Exception($"PUT externo falló para Solicitation con RequestId {solicitation.RequestId}");
        }
        else if (!request.CompletedTask.IsSolicitation && request.CompletedTask.IncidenceId.HasValue)
        {
            var incidence = await _incidenceRepository.GetByIdAsync(request.CompletedTask.IncidenceId.Value);
            if (incidence == null)
                throw new Exception($"Incidence con ID {request.CompletedTask.IncidenceId.Value} no existe.");
            total += company.Price;
            foreach (var material in materials)
                total += material.Cost;
            // Llenar campos desde Incidence
            completedTask.IssueTypeId = incidence.IssueTypeId;
            completedTask.IssueType = incidence.IssueType;
            completedTask.ApartmentId = incidence.ApartmentId;
            completedTask.Surface = incidence.Surface;
            var apartment = await _apartmentRepository.GetByIdAsync(incidence.ApartmentId);
            completedTask.Address = apartment?.Address ?? "";
            // Simulación de PUT a API externa para Incidence (antes de guardar)
            simulatedPutResult = await SimulatePutToExternalApiAsync($"https://api.incidences.com/incidences/{incidence.Id}", incidence.Id);
            if (simulatedPutResult != "OK")
                throw new Exception($"PUT externo falló para Incidence con Id {incidence.Id}");
        }
        else
        {
            throw new Exception("Debe especificar SolicitationId o IncidenceId correctamente.");
        }
        completedTask.Price = (double)total;
        completedTask.IsSolicitation = request.CompletedTask.IsSolicitation;

        _completedTaskRepository.Add(completedTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return completedTask.Id;
    }

    // Simulación de PUT a una API externa
    private Task<string> SimulatePutToExternalApiAsync(string url, object data)
    {
        // Si es Solicitation, data es un objeto anónimo con IdRequest y Total
        if (data is int id && id < 0)
            return Task.FromResult("ERROR");
        if (data.GetType().GetProperty("IdRequest") is not null)
        {
            var idRequest = (int)data.GetType().GetProperty("IdRequest")!.GetValue(data)!;
            if (idRequest < 0)
                return Task.FromResult("ERROR");
        }
        // Simulación de llamanda
        return Task.FromResult("OK");
    }
}
