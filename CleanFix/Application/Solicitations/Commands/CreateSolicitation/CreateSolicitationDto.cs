using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Commands.CreateSolicitation;
public class CreateSolicitationDto
{
    [StringLength(200, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 200 caracteres.")]
    public string? Description { get; set; }
    [StringLength(150, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 150 caracteres.")]
    public string Address { get; set; }
    [Required]
    public int IssueTypeId { get; set; } // <-- Que sea un numero entre 1 y 7
    [Required]
    public string BuildingCode { get; set; }
    [Required]
    public int ApartmentAmount { get; set; } // Nuevo campo solicitado
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateSolicitationDto, Solicitation>();
        }
    }
}
