
using WebApi.Entidades;

namespace WebApi.Models;

public class MaterialDto
{
    public Guid Id { get; set; } // Identificador único del material
    public string Name { get; set; }
    public float Cost { get; set; }
    public bool Available { get; set; }
    public IssueType Issue { get; set; }
}
