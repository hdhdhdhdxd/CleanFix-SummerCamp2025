using AutoMapper;
using Domain.Entities;

namespace Application.Companies.Queries.GetCompanies;
public class GetCompaniesDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Number { get; set; }
    public string? Email { get; set; }
    public IssueType? Type { get; set; }
    public decimal? Price { get; set; }
    public int? WorkTime { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Company, GetCompaniesDto>();
        }
    }
}
