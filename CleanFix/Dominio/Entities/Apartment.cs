<<<<<<< HEAD:CleanFix/Dominio/Entities/Apartment.cs
﻿using Dominio.Common.Interfaces;
=======
﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.Entidades;
>>>>>>> 3860d99b48ff67cce57bb6505b0d7bf413574b20:CleanFix/WebApi/Entidades/Apartment.cs

namespace WebApi.Entidades;

public class Apartment : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public int FloorNumber { get; set; } // Piso del apartamento

    [Required(ErrorMessage = "La dirección es obligatoria."), MaxLength(100, ErrorMessage = "La direccion no puede pasar de 100 caracteres")]
    public string Address { get; set; } // Dirección del apartamento
    public double Surface { get; set; } // Superficie del apartamento
    public int RoomNumber { get; set; } // Número de habitaciones
    public int BathroomNumber { get; set; } // Número de baños
}
