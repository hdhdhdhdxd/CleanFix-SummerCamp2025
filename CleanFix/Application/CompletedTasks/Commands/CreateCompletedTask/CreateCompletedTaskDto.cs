using AutoMapper;
using Domain.Entities;

namespace Application.CompletedTasks.Commands.CreateCompletedTask;
public class CreateCompletedTaskDto
{
    public int? SolicitationId { get; set; } 
    public int? IncidenceId { get; set; } 
    public int CompanyId { get; set; }
    public int[] MaterialIds { get; set; } = [];
    public bool IsSolicitation { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateCompletedTaskDto, CompletedTask>()
                .ForMember(dest => dest.Price, opt => opt.Ignore()) // Ignore Price - calculated manually
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore()) // Set from Solicitation/Incidence
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore()) // Set manually
                .ForMember(dest => dest.CompletionDate, opt => opt.Ignore()) // Set manually
                .ForMember(dest => dest.Surface, opt => opt.Ignore()) // Set from Solicitation/Incidence
                .ForMember(dest => dest.Materials, opt => opt.Ignore()) // Set manually
                .ForMember(dest => dest.Company, opt => opt.Ignore()) // Set manually
                .ForMember(dest => dest.IssueType, opt => opt.Ignore()) // Set from Solicitation/Incidence
                .ForMember(dest => dest.IssueTypeId, opt => opt.Ignore()) // Set from Solicitation/Incidence
                .ForMember(dest => dest.Apartment, opt => opt.Ignore()) // Set manually
                .ForMember(dest => dest.ApartmentId, opt => opt.Ignore()) // Set from Incidence
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore()); // Database generated
        }
    }
}
