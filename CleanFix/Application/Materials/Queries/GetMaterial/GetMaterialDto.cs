using AutoMapper;
using Domain.Entities;

namespace Application.Materials.Queries.GetMaterial;
public class GetMaterialDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Cost { get; set; }
    public bool Available { get; set; }
    public string IssueType { get; set; } // Nuevo campo para el nombre
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Material, GetMaterialDto>()
                .ForMember(dest => dest.IssueType, opt => opt.MapFrom(src => src.IssueType.Name != null));
        }
    }
}
