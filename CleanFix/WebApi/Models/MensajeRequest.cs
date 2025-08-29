namespace WebApi.Models
{
    public class MensajeRequest
    {
        public string Mensaje { get; set; } // Debe coincidir con el JSON recibido
        public List<string> Historial { get; set; } // Nuevo campo opcional para el historial
    }
}
