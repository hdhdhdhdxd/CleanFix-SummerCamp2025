using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Materials.Commands.CreateMaterial;
public class CreateMaterialDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Cost { get; set; }
    [Required]
    public bool Available { get; set; }
    [Required]
    public int IssueTypeId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateMaterialDto, Material>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => Math.Round(src.Cost, 2)));
        }
    }
}
