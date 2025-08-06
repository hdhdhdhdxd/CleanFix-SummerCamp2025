using Microsoft.SemanticKernel;

public class FacturaPlugin
{
    [KernelFunction]
    public string ConsultarEstado(string id) => $"La factura {id} esta pagada.";
    [KernelFunction]
    public string CrearPrefactura(string user) => $"Prefactura creada para {user}.";
}
