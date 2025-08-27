using AutoMapper;
using Domain.Entities;

namespace Application.Incidences.Queries.GetPaginatedIncidences;
public class GetPaginatedIncidenceDto
{
    public int Id { get; set; }
    public string IssueType { get; set; } // Renombrado y tipado como string
    public int IssueTypeId { get; set; } // Identificador del tipo de incidencia
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public Priority Priority { get; set; }
    public byte[] RowVersion { get; set; } // Para concurrencia
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Incidence, GetPaginatedIncidenceDto>()
                .ForMember(dest => dest.IssueType, opt => opt.MapFrom(src => src.IssueType != null ? src.IssueType.Name : null)); // Las demás propiedades se mapean automáticamente
        }
    }
}
