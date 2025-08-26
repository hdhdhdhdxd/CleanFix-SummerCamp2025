using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Queries.GetPaginatedIncidences;
public class GetPaginatedIncidenceDto
{
    public int Id { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public Priority Priority { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Incidence, GetPaginatedIncidenceDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.IssueType != null ? src.IssueType.Name : null));
        }
    }
}
