using Domain.Common.Interfaces;

namespace Domain.Entities;

public class Solicitation : IEntity
{
    public int Id { get; set; } // Identificador único de la solicitud
    public string Address { get; set; }
    public Guid ApartmentId { get; set; }
    public Company Company { get; set; }
    public DateTime Date { get; set; }
    public double Price { get; set; }
    public double Duration { get; set; }
    public IssueType Type { get; set; }
    public List<Material> Materials { get; set; }
    public bool IsRequest { get; set; }
    public User User { get; set; }
    public int Surface { get; set; } // Superficie del apartamento
}
