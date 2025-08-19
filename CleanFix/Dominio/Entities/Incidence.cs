using Domain.Common.Interfaces;

namespace Domain.Entities;

public class Incidence : IEntity
{
    public int Id { get; set; } // Identificador único de la incidencia
    public IssueType Type { get; set; } // Tipo de incidencia (e.g., "Electricidad", "Fontanería", etc.)
    public DateTime Date { get; set; } // Fecha de la incidencia
    public string Status { get; set; } // Estado de la incidencia (e.g., "Pendiente", "En Progreso", "Resuelta")
    public string Description { get; set; } // Descripción de la incidencia
    public int ApartmentId { get; set; } // Apartamento asociado a la incidencia
    public int Surface { get; set; } // Superficie del apartamento
    public Priority Priority { get; set; } // Prioridad de la incidencia (e.g., "Baja", "Media", "Alta", "Crítica")
}

public enum Priority
{

    Low,      // Baja prioridad
    Medium,   // Prioridad media
    High,     // Alta prioridad
    Critical  // Prioridad crítica
}
