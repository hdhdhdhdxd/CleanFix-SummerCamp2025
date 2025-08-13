using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Commands.UpdateIncidence;
public class UpdateIncidenceDto
{
    public int Id { get; set; }
    public IssueType Type { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public int ApartmentId { get; set; }
    public Priority Priority { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateIncidenceDto, Incidence>();
        }
    }
}
