using AutoMapper;
using Domain.Entities;

namespace Application.CompletedTasks.Queries.GetPaginatedCompletedTasks;
public class GetPaginatedCompletedTaskDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime CompletionDate { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CompletedTask, GetPaginatedCompletedTaskDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : null));
        }
    }
}
