using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Entities;

namespace Application.CompletedTasks.Queries.GetCompletedTask;
public class GetCompletedTaskDto
{
    public int Id { get; set; }
    public string Address { get; set; }
    public CompanyDto Company { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime CompletionDate { get; set; }
    public double Price { get; set; }
    public string IssueType { get; set; }
    public List<string> Materials { get; set; }
    public bool IsSolicitation { get; set; }
    public int Surface { get; set; }

    public class CompanyDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CompletedTask, GetCompletedTaskDto>()
                .ForMember(d => d.IssueType, opt => opt.MapFrom(s => s.IssueType != null ? s.IssueType.Name : null));
            CreateMap<Material, string>().ConvertUsing(m => m.Name);
            CreateMap<Company, CompanyDto>();
        }
    }
}
