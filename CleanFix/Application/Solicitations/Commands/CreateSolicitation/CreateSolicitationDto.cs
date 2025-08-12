using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Commands.CreateSolicitation;
public class CreateSolicitationDto
{
    public string Description { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ApartmentId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateSolicitationDto, Solicitation>();
        }
    }
}
