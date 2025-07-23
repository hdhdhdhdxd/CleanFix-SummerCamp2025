
// See https://aka.ms/new-console-template for more information


public class Empresa
{
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Tipo { get; set; } // Ejemplo: "Electricidad", "Fontanería", etc.
    public decimal Coste { get; set; }
    public TimeSpan TiempoTrabajo { get; set; }

    // Constructor estándar
    public Empresa(string nombre, string direccion, string telefono, string email, string tipo, decimal coste, TimeSpan tiempoTrabajo)
    {
        Nombre = nombre;
        Direccion = direccion;
        Telefono = telefono;
        Email = email;
        Tipo = tipo;
        Coste = coste;
        TiempoTrabajo = tiempoTrabajo;
    }
}
