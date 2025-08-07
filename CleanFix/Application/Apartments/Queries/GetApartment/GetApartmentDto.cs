using AutoMapper;
using WebApi.Entidades;

namespace Application.Apartments.Queries.GetApartment;
public class GetApartmentDto
{
    public Guid Id { get; set; }
    public int FloorNumber { get; set; } // Piso del apartamento
    public string Address { get; set; } // Dirección del apartamento
    public double Surface { get; set; } // Superficie del apartamento
    public int RoomNumber { get; set; } // Número de habitaciones
    public int BathroomNumber { get; set; } // Número de baños
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Apartment, GetApartmentDto>();
        }
    }
}
