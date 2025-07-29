namespace WebApi.Models;

public class CompanyDto
{
    public int Id { get; set; } // Identificador único de la empresa
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Number { get; set; }
    public string? Email { get; set; }
    public string? Type { get; set; } // Ejemplo: "Electricidad", "Fontanería", etc.
    public decimal? Price { get; set; }
    public int? WorkTime { get; set; }

}
