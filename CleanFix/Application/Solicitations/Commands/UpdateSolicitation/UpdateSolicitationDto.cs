using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Commands.UpdateSolicitation;
public class UpdateSolicitationDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ApartmentId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateSolicitationDto, Solicitation>();
        }
    }
}
