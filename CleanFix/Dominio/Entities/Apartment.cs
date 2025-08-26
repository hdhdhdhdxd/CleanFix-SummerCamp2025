using System.ComponentModel.DataAnnotations;
using Domain.Common.Interfaces;

namespace Domain.Entities;

public class Apartment : IEntity
{
    [Key]
    public int Id { get; set; }
    public int FloorNumber { get; set; } // Piso del apartamento

    [Required(ErrorMessage = "La dirección es obligatoria."), MaxLength(100, ErrorMessage = "La direccion no puede pasar de 100 caracteres")]
    public string Address { get; set; } // Dirección del apartamento
    public double Surface { get; set; } // Superficie del apartamento
    public int RoomNumber { get; set; } // Número de habitaciones
    public int BathroomNumber { get; set; } // Número de baños
    public DateTime CreationDate { get; set; }
}
