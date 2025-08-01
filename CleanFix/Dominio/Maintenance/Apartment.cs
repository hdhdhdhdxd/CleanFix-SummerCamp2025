using Dominio.Common.Interfaces;

namespace Dominio.Maintenance
{
    public class Apartment : IEntity
    {
        public int Id { get; set; }
        public string Address { get; set; }
        string Surface { get; set; }
        IssueType Type { get; set; }
    }
}
