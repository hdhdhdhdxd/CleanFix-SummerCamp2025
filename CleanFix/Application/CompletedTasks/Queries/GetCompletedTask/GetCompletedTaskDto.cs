using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.CompletedTasks.Queries.GetCompletedTask;
public class GetCompletedTaskDto
{
    public int Id { get; set; }
    public string Address { get; set; }
    public ApartmentDto Apartment { get; set; }
    public CompanyDto Company { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime CompletionDate { get; set; }
    public double Price { get; set; }
    public string IssueType { get; set; }
    public List<MaterialDto> Materials { get; set; }
    public bool IsSolicitation { get; set; }
    public int Surface { get; set; }
    // Puedes agregar más hijos si lo necesitas

    public class MaterialDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool Available { get; set; }
        public int IssueTypeId { get; set; }
        public int CompletedTaskId { get; set; }
    }

    public class CompanyDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public int IssueTypeId { get; set; }
        public decimal Price { get; set; }
        public int WorkTime { get; set; }
    }

    public class ApartmentDto { 
        public int Id { get; set; }
        public int FloorNumber { get; set; } // Piso del apartamento
        public string Address { get; set; } // Dirección del apartamento
        public double Surface { get; set; } // Superficie del apartamento
        public int RoomNumber { get; set; } // Número de habitaciones
        public int BathroomNumber { get; set; } // Número de baños
        public DateTime CreationDate { get; set; }
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CompletedTask, GetCompletedTaskDto>()
                .ForMember(d => d.IssueType, opt => opt.MapFrom(s => s.IssueType != null ? s.IssueType.Name : null));
            CreateMap<Material, MaterialDto>();
            CreateMap<Company, CompanyDto>();
            CreateMap<Apartment, ApartmentDto>();
        }
    }
}
