using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Queries.GetSolicitation;
public class GetSolicitationDto
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string Address { get; set; }
    public string? Status { get; set; }
    public double MaintenanceCost { get; set; }
    public int IssueTypeId { get; set; } // Identificador del tipo de incidencia
    public IssueType IssueType { get; set; } // Nuevo campo para el nombre
    public string BuildingCode { get; set; } // Código del edificio
    public byte[] RowVersion { get; set; } // Para concurrencia
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Solicitation, GetSolicitationDto>();
        }
    }
}
