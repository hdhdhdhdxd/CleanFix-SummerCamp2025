namespace WebApi.Entidades;

public class Solicitation
{
    public Guid Id { get; set; } // Identificador único de la aplicación
    public Apartment? Apartment { get; set; } // Apartamento asociado a la aplicación
    public Company? Company { get; set; } // Empresa asociada a la aplicación
    public DateTime Date { get; set; } // Fecha de la aplicación
    public double Price { get; set; } // Precio de la aplicación    
    public double Duration { get; set; } // Duración de la aplicación en horas
    public string Address { get; set; } // Dirección de la aplicación
    public IssueType Type { get; set; } // Tipo de problema asociado a la aplicación
    public List<Material> Materials { get; set; }
    public bool IsRequest { get; set; } // Indica si es una solicitud de servicio o una oferta de trabajo
}
