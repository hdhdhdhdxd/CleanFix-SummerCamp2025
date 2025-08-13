using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Queries.GetPaginatedSolicitations;
public class GetPaginatedSolicitationDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int CompanyId { get; set; }
    public int ApartmentId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Solicitation, GetPaginatedSolicitationDto>();
        }
    }
}
