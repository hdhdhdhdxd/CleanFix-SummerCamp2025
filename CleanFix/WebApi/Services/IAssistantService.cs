namespace WebApi.Services
{
    public interface IAssistantService
    {
        Task<string> ProcesarMensajeAsync(string mensaje);
    }
}
