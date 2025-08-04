using WebApi.Entidades;
namespace WebApi.Models;

public class UserDto
{
    public Guid Id { get; set; } // Identificador único del usuario
    public string? Name { get; set; } // Nombre del usuario
    public List<Solicitation> Applications { get; set; } // Lista de aplicaciones asociadas al usuario
    public ApartmentDto? Apartment { get; set; } // Apartamento asociado al usuario
}
