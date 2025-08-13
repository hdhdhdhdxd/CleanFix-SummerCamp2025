using AutoMapper;
using Domain.Entities;

namespace Application.Materials.Queries.GetPaginatedMaterials;
public class GetPaginatedMaterialDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public bool Available { get; set; }
    public IssueType Issue { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Material, GetPaginatedMaterialDto>();
        }
    }
}
