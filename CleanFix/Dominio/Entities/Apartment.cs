using Dominio.Common.Interfaces;

namespace WebApi.Entidades;

public class Apartment : IEntity
{
    public Guid Id { get; set; }
    public int FloorNumber { get; set; } // Piso del apartamento
    public string Address { get; set; } // Dirección del apartamento
    public double Surface { get; set; } // Superficie del apartamento
    public int RoomNumber { get; set; } // Número de habitaciones
    public int BathroomNumber { get; set; } // Número de baños
}
