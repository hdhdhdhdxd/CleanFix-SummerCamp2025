using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.Companies.Commands.UpdateCompany;
public class UpdateCompanyDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Number { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public int IssueTypeId { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int WorkTime { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateCompanyDto, Company>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Round(src.Price, 2)));
        }
    }
}
