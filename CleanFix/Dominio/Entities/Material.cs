using Domain.Common.Interfaces;

namespace Domain.Entities;

public class Material : IEntity
{
    public int Id { get; set; } // Identificador único del material
    public string Name { get; set; }
    public float Cost { get; set; }
    public bool Available { get; set; }
    public IssueType Issue { get; set; }
}
