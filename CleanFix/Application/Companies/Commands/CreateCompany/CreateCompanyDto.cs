using AutoMapper;
using WebApi.Entidades;

namespace Application.Companies.Commands.CreateCompany;
public class CreateCompanyDto
{
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
            CreateMap<CreateCompanyDto, Company>();
        }
    }
}
