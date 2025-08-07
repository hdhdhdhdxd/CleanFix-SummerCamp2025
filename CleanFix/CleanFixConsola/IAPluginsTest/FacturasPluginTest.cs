using Microsoft.SemanticKernel;

public class FacturasPluginTest
{
    [KernelFunction]
    public string ConsultarEstado(string id) => $"La factura {id} esta pagada.";
    [KernelFunction]
    public string CrearPrefactura(string user) => $"Prefactura creada para {user}.";
}

