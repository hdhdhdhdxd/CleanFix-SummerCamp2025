using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Commands.CreateIncidence;
public class CreateIncidenceDto
{
    [Required]
    public int IssueTypeId { get; set; } // Del 1 al 7
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El estado debe tener entre 3 y 50 caracteres.")]
    public string Description { get; set; }
    [Required]
    public string Address { get; set; } // Dirección del apartamento
    [Required]
    public int Surface { get; set; } // Superficie del apartamento
    [Required]
    public Priority Priority { get; set; }  // Del 1 al 4
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateIncidenceDto, Incidence>();
        }
    }
}
