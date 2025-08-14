using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Materials.Commands.UpdateMaterial;

public class UpdateMaterialDto
{
    public int Id { get; set; }
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; }
    [Range(1, 10000, ErrorMessage = "El coste debe estar entre 1 y 10000.")]
    public float Cost { get; set; }
    public bool Available { get; set; }
    [Required]
    public IssueType Issue { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Material, UpdateMaterialDto>();
        }
    }
}
