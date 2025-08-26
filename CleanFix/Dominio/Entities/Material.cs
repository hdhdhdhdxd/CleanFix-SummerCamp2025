using Domain.Common.Interfaces;

namespace Domain.Entities;

public class Material : IEntity
{
    public int Id { get; set; } // Identificador único del material
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public bool Available { get; set; }
    public int IssueTypeId { get; set; }
    public IssueType IssueType { get; set; }
}
