using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Queries.GetIncidences;
public class GetIncidencesDto
{
    public int Id { get; set; }
    public int IssueTypeId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public int ApartmentId { get; set; }
    public Priority Priority { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Incidence, GetIncidencesDto>();
        }
    }
}
