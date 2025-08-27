using Application.Incidences.Queries.GetPaginatedIncidences;
using AutoMapper;
using Domain.Entities;

namespace Application.Materials.Queries.GetRandomMaterialsByIssueType
{
    public class GetRandomMaterialDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool Available { get; set; }
        public int IssueTypeId { get; set; }
        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Material, GetRandomMaterialDto>();
            }
        }
    }
}
