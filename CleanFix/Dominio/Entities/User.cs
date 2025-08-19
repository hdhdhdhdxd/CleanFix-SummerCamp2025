using Domain.Common.Interfaces;

namespace Domain.Entities;

public class User : IEntity
{
    public int Id { get; set; } // Identificador único del usuario
    public string? Name { get; set; } // Nombre del usuario
    public List<CompletedTask> Applications { get; set; } // Lista de aplicaciones asociadas al usuario
    public Apartment? Apartment { get; set; } // Apartamento asociado al usuario
}
