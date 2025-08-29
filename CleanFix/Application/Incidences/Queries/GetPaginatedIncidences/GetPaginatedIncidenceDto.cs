using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Queries.GetPaginatedIncidences;
public class GetPaginatedIncidenceDto
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Incidence, GetPaginatedIncidenceDto>()
                .ForMember(dest => dest.IssueType, opt => opt.MapFrom(src => src.IssueType != null ? src.IssueType.Name : null));
        }
    }
}
