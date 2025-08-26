using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Queries.GetPaginatedSolicitations;
public class GetPaginatedSolicitationDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Status { get; set; }
    public string IssueType { get; set; }
    public string Address { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Solicitation, GetPaginatedSolicitationDto>()
                .ForMember(
                    dest => dest.IssueType,
                    opt => opt.MapFrom(src => src.IssueType.Name)
                );
        }
    }
}
