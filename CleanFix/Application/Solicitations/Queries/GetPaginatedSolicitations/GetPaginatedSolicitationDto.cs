using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Queries.GetPaginatedSolicitations;
public class GetPaginatedSolicitationDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Status { get; set; }
    public IssueType Type { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Solicitation, GetPaginatedSolicitationDto>();
        }
    }
}
