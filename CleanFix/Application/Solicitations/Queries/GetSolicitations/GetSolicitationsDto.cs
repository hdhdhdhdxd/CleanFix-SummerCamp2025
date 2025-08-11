using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Queries.GetSolicitations;
public class GetSolicitationsDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ApartmentId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Solicitation, GetSolicitationsDto>();
        }
    }
}
