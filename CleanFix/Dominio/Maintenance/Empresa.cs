// See https://aka.ms/new-console-template for more information

public class Empresa
{
    public int Id { get; set; } // Identificador único de la empresa
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Tipo { get; set; } // Ejemplo: "Electricidad", "Fontanería", etc.
    public decimal Coste { get; set; }
    public TimeSpan TiempoTrabajo { get; set; }

    // Constructor estándar
    public Empresa(int id, string nombre, string direccion, string telefono, string email, string tipo, decimal coste, TimeSpan tiempoTrabajo)
    {
        Id = id;
        Nombre = nombre;
        Direccion = direccion;
        Telefono = telefono;
        Email = email;
        Tipo = tipo;
        Coste = coste;
        TiempoTrabajo = tiempoTrabajo;
    }
    public Empresa()
    {
        // Constructor por defecto
    }
     
    // Método para mostrar información de la empresa
    public string MostrarInformacion()
    {
        return $"Id: {Id}, Nombre: {Nombre}, Dirección: {Direccion}, Teléfono: {Telefono}, Email: {Email}, Coste: {Coste:C}, Tiempo de Trabajo: {TiempoTrabajo.TotalHours} horas";
    }

}
