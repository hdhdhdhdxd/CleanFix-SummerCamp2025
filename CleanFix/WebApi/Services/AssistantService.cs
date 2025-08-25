using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;
using CleanFix.Plugins;
using System.Collections.Generic;
using System.Diagnostics;

namespace WebApi.Services
{
    public interface IAssistantService
    {
        Task<string> ProcesarMensajeAsync(string mensaje);
    }

    public class AssistantService : IAssistantService
    {
        private readonly Kernel _kernel;
        private readonly string _empresasJson;
        private readonly string _materialesJson;
        private readonly string _promptTemplate = @"
Eres un asistente inteligente que responde preguntas sobre empresas y materiales.

Tienes la siguiente información de empresas (companies) en formato JSON: {{$empresas}} Cada empresa tiene propiedades como: Id, Name, Type (tipo), Price.

También tienes la siguiente información de materiales (materials) en formato JSON: {{$materiales}} Cada material tiene propiedades como: Id, Name, Issue (tipo), Available (disponible).

Usa esta información para responder la pregunta del usuario: {{$pregunta}} Responde de forma clara y útil.

Si la pregunta no tiene relación con los datos, responde que no puedes ayudar.
";

        public AssistantService(IConfiguration config)
        {
            var endpoint = config["AzureOpenAI:Endpoint"];
            var apiKey = config["AzureOpenAI:ApiKey"];
            var connectionString = config.GetConnectionString("DefaultConnection");

            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: "gpt-4.1",
                endpoint: endpoint,
                apiKey: apiKey
            );

            // Carga datos de empresas y materiales al inicializar
            var dbPlugin = new DBPluginTestPG(connectionString);
            var empresasResponse = dbPlugin.GetAllEmpresas();
            var materialesResponse = dbPlugin.GetAllMaterials();

            // LOG para depuración
            Debug.WriteLine($"[AssistantService] Empresas count: {empresasResponse.Data?.Count ?? 0}");
            Debug.WriteLine($"[AssistantService] Materiales count: {materialesResponse.Data?.Count ?? 0}");

            // Si no hay datos, lanza excepción para ver el error en la API
            if (empresasResponse.Data == null || empresasResponse.Data.Count == 0)
            {
                Debug.WriteLine("[AssistantService] ¡ATENCIÓN! No se encontraron empresas en la base de datos.");
            }
            if (materialesResponse.Data == null || materialesResponse.Data.Count == 0)
            {
                Debug.WriteLine("[AssistantService] ¡ATENCIÓN! No se encontraron materiales en la base de datos.");
            }

            _empresasJson = JsonSerializer.Serialize(empresasResponse.Data ?? new List<CompanyIa>());
            _materialesJson = JsonSerializer.Serialize(materialesResponse.Data ?? new List<MaterialIa>());

            _kernel = builder.Build();
        }

        public async Task<string> ProcesarMensajeAsync(string mensaje)
        {
            // LOG para depuración
            Debug.WriteLine($"[AssistantService] Pregunta recibida: {mensaje}");
            Debug.WriteLine($"[AssistantService] Empresas JSON: {_empresasJson}");
            Debug.WriteLine($"[AssistantService] Materiales JSON: {_materialesJson}");

            var promptFunction = _kernel.CreateFunctionFromPrompt(_promptTemplate);

            var kernelArgs = new KernelArguments
            {
                ["empresas"] = _empresasJson,
                ["materiales"] = _materialesJson,
                ["pregunta"] = mensaje
            };

            var responseData = await promptFunction.InvokeAsync(_kernel, kernelArgs);
            var respuesta = responseData.GetValue<string>();
            Debug.WriteLine($"[AssistantService] Respuesta LLM: {respuesta}");
            return respuesta;
        }
    }
}
