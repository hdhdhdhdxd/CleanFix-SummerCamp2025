using AutoMapper;
using Domain.Entities;

namespace Application.Solicitations.Queries.GetPaginatedSolicitations;
public class GetPaginatedSolicitationDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string IssueType { get; set; }
    public int IssueTypeId { get; set; } // Identificador del tipo de incidencia
    public string Address { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Solicitation, GetPaginatedSolicitationDto>()
                .ForMember(
                    dest => dest.IssueType,
                    opt => opt.MapFrom(src => src.IssueType.Name)
                ); // Las demás propiedades se mapean automáticamente
        }
    }
}
