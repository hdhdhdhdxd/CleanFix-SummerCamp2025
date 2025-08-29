using System.ComponentModel.DataAnnotations;
using Domain.Common.Interfaces;

namespace Domain.Entities;

public class CompletedTask : IEntity
{
    public int Id { get; set; } // Identificador único de la tarea completada
    public string Address { get; set; }
    public int? ApartmentId { get; set; }
    public Apartment? Apartment { get; set; }
    public Company Company { get; set; }
    public int CompanyId { get; set; } // Agregada propiedad para la clave foránea de Company
    public DateTime CreationDate { get; set; } // Cambié el nombre de la propiedad a CreationDate
    public DateTime CompletionDate { get; set; }
    public double Price { get; set; }
    public int IssueTypeId { get; set; }
    public IssueType IssueType { get; set; } // Agregada propiedad de navegación
    public List<Material> Materials { get; set; }
    public bool IsSolicitation { get; set; }
    public double Surface { get; set; } // Superficie del apartamento
    [Timestamp]
    public byte[] RowVersion { get; set; }
}
