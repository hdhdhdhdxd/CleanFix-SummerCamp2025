using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Apartments.Commands.UpdateApartment;
public class UpdateApartmentDto
{
    public int Id { get; set; }
    [Range(1, 100, ErrorMessage = "El número de piso debe estar entre 1 y 100.")]
    public int FloorNumber { get; set; } // Piso del apartamento
    [StringLength(100, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 100 caracteres.")]
    public string Address { get; set; } // Dirección del apartamento
    [Range(10, 1000, ErrorMessage = "La superficie debe estar entre 10 y 1000 m².")]
    public double Surface { get; set; } // Superficie del apartamento
    [Range(1, 20, ErrorMessage = "El número de habitaciones debe estar entre 1 y 20.")]
    public int RoomNumber { get; set; } // Número de habitaciones
    [Range(1, 10, ErrorMessage = "El número de baños debe estar entre 1 y 10.")]
    public int BathroomNumber { get; set; } // Número de baños
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateApartmentDto, Apartment>();
        }
    }
}
