using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Commands.CreateSolicitation;
public class CreateSolicitationDto
{
    public string Description { get; set; }
    public int CompanyId { get; set; }
    public int ApartmentId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateSolicitationDto, Solicitation>();
        }
    }
}
