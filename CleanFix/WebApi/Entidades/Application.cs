namespace WebApi.Entidades;

public class Application
{
    public int Id { get; set; } // Identificador único de la aplicación
    public Company? Company { get; set; } // Empresa asociada a la aplicación
    public DateTime Date { get; set; } // Fecha de la aplicación
    public double Price { get; set; } // Precio de la aplicación    
    public double Duration { get; set; } // Duración de la aplicación en horas
    public string Address { get; set; } // Dirección de la aplicación
}
