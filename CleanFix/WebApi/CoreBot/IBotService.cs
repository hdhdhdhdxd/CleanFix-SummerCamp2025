namespace WebApi.CoreBot
{
    public interface IBotService
    {
        Task<string> ProcesarMensajeAsync(string mensaje);
    }
}
