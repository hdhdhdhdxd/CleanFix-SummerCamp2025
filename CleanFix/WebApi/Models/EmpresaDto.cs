namespace WebApi.Models;

public class EmpresaDto
{
    public int Id { get; set; } // Identificador único de la empresa
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Tipo { get; set; } // Ejemplo: "Electricidad", "Fontanería", etc.
    public decimal Coste { get; set; }
    public TimeSpan TiempoTrabajo { get; set; }

}
