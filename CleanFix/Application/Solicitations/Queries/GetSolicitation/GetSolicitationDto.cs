using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Queries.GetSolicitation;
public class GetSolicitationDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ApartmentId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Solicitation, GetSolicitationDto>();
        }
    }
}
