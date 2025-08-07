using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Apartments.Commands.PostApartments;
using AutoMapper;
using WebApi.Entidades;

namespace Application.Apartments.Commands.PutApartments;
public class PutApartmentDto
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
            CreateMap<Apartment, PutApartmentDto>();
        }
    }
}
