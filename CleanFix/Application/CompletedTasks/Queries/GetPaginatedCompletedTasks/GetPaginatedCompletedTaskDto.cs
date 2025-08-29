using AutoMapper;
using Domain.Entities;

namespace Application.CompletedTasks.Queries.GetPaginatedCompletedTasks;
public class GetPaginatedCompletedTaskDto
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string CompanyName { get; set; }
    public string IssueType { get; set; } // Propiedad IssueType como string
    public bool IsSolicitation { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime CompletionDate { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CompletedTask, GetPaginatedCompletedTaskDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null))
                .ForMember(dest => dest.IssueType, opt => opt.MapFrom(src => src.IssueType != null ? src.IssueType.Name : null));
        }
    }
}
