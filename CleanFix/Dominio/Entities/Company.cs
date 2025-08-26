using Domain.Common.Interfaces;

namespace Domain.Entities;

public class Company : IEntity
{
    public int Id { get; set; } // Identificador único de la empresa
    public string Name { get; set; }
    public string Address { get; set; }
    public string Number { get; set; }
    public string Email { get; set; }
    public IssueType IssueType { get; set; }
    public decimal Price { get; set; }
    public int WorkTime { get; set; }
}
