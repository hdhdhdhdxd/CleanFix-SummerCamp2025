using Dominio.Common.Interfaces;

namespace WebApi.Entidades;

public class User : IEntity
{
    public Guid Id { get; set; } // Identificador único del usuario
    public string? Name { get; set; } // Nombre del usuario
    public List<Solicitation> Applications { get; set; } // Lista de aplicaciones asociadas al usuario
    public Apartment? Apartment { get; set; } // Apartamento asociado al usuario
}
