using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Commands.UpdateSolicitation;
public class UpdateSolicitationDto
{
    public int Id { get; set; }
    [StringLength(200, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 200 caracteres.")]
    public string? Description { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [StringLength(150, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 150 caracteres.")]
    public string Address { get; set; }
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El estado debe tener entre 3 y 50 caracteres.")]
    public string? Status { get; set; }
    [Range(1, 10000, ErrorMessage = "El coste de mantenimiento debe estar entre 1 y 10000.")]
    public double MaintenanceCost { get; set; }
    [Required]
    public int IssueTypeId { get; set; } // <-- Usar el Id, no el objeto
    public byte[] RowVersion { get; set; } // Para concurrencia
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateSolicitationDto, Solicitation>();
        }
    }
}
