using AutoMapper;
using WebApi.Entidades;

namespace Application.Materials.Queries.GetMaterial;
public class GetMaterialDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Cost { get; set; }
    public bool Available { get; set; }
    public IssueType Issue { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Material, GetMaterialDto>();
        }
    }
}
