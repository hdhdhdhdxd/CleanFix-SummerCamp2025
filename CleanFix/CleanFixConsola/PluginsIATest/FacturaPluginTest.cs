using Microsoft.SemanticKernel;

public class FacturaPluginTest
{
    [KernelFunction]
    public string ConsultarEstado(string id) => $"La factura {id} esta pagada.";
    [KernelFunction]
    public string CrearPrefactura(string user) => $"Prefactura creada para {user}.";
}
