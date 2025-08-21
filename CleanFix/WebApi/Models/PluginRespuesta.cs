namespace WebApi.CoreBot.Models
{
    public class PluginRespuesta<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}
