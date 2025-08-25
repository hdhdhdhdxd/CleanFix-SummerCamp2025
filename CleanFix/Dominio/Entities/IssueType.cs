using Domain.Common.Interfaces;

namespace Domain.Entities;

public class IssueType : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}
