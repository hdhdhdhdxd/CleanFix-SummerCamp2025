using AutoMapper;
using WebApi.Entidades;

namespace Application.Apartments.Commands.PostApartments;
public class PostApartmentDto
{
    public int FloorNumber { get; set; } // Piso del apartamento
    public string Address { get; set; } // Dirección del apartamento
    public double Surface { get; set; } // Superficie del apartamento
    public int RoomNumber { get; set; } // Número de habitaciones
    public int BathroomNumber { get; set; } // Número de baños
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Apartment, PostApartmentDto>();
        }
    }
}
