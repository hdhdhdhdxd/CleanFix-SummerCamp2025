using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Materials.Commands.UpdateMaterial;
public class UpdateMaterialDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal CostPerSquareMeter { get; set; }
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
            CreateMap<UpdateMaterialDto, Material>()
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => Math.Round(src.Cost, 2)));
        }
    }
}
