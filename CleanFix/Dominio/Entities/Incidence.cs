using System.ComponentModel.DataAnnotations;
using Domain.Common.Interfaces;

namespace Domain.Entities;

public class Incidence : IEntity
{
    public int Id { get; set; } // Identificador único de la incidencia
    public int IncidenceId { get; set; } // Id de CozyHouse
    public int IssueTypeId { get; set; }
    public IssueType IssueType { get; set; } // Tipo de incidencia (Id)
    public DateTime Date { get; set; } // Fecha de la incidencia
    public string Description { get; set; } // Descripción de la incidencia
    public string Address { get; set; } // Dirección del apartamento
    public int Surface { get; set; } // Superficie del apartamento
    public Priority Priority { get; set; } // Prioridad de la incidencia (e.g., "Baja", "Media", "Alta", "Crítica")
    [Timestamp]
    public byte[] RowVersion { get; set; } // Propiedad para controlar la concurrencia
}

public enum Priority
{
    Low,      // Baja prioridad
    Medium,   // Prioridad media
    High,     // Alta prioridad
    Critical  // Prioridad crítica
}
