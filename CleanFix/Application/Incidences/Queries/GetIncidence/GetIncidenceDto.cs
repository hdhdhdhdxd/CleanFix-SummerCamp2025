using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Queries.GetIncidence;
public class GetIncidenceDto
{
    public int Id { get; set; } 
    public IssueType IssueType { get; set; } 
    public DateTime Date { get; set; } 
    public string Description { get; set; }
    public string Address { get; set; } 
    public int Surface { get; set; } 
    public Priority Priority { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Incidence, GetIncidenceDto>();
        }
    }
}
