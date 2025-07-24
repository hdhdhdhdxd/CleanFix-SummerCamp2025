namespace Dominio.Maintenance
{
    public class Apartment
    {
        int id { get; }
        string address { get; set; }
        string surface { get; set; }
        IssueType Type { get; set; }
    }
}
