using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Commands.UpdateIncidence;
public class UpdateIncidenceDto
{
    public int Id { get; set; }
    [Required]
    public IssueType Type { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El estado debe tener entre 3 y 50 caracteres.")]
    public string Status { get; set; }
    [StringLength(200, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 200 caracteres.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "El ApartmentId es obligatorio.")]
    public int ApartmentId { get; set; }
    [Required]
    public Priority Priority { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateIncidenceDto, Incidence>();
        }
    }
}
