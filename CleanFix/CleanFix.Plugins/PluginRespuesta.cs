namespace WebApi.CoreBot.Models
{
    public class PluginRespuesta
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public object Data { get; set; }
    }
}
