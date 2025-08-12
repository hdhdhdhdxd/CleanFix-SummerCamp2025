using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Commands.CreateIncidence;
public class CreateIncidenceDto
{
    public IssueType Type { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public Guid ApartmentId { get; set; }
    public Priority Priority { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateIncidenceDto, Incidence>();
        }
    }
}
