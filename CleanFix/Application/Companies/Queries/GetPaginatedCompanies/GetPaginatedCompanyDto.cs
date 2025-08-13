using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Apartments.Queries.GetPaginatedApartment;
using Application.Companies.Queries.GetCompany;
using AutoMapper;
using Domain.Entities;

namespace Application.Companies.Queries.GetPaginatedCompanies;
public class GetPaginatedCompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Number { get; set; }
    public string Email { get; set; }
    public IssueType Type { get; set; }
    public decimal Price { get; set; }
    public int WorkTime { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Company, GetPaginatedCompanyDto>();
        }
    }
}
