using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using CleanFix.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace WebApi.Services
{
    public class AssistantService : IAssistantService
    {
        private readonly Kernel _kernel;
        private readonly string _empresasJson;
        private readonly string _materialesJson;
        private readonly string _promptTemplate;

        public AssistantService(IConfiguration config)
        {
            var builder = Kernel.CreateBuilder();
            _promptTemplate = @"Eres un asistente inteligente que responde preguntas sobre empresas y materiales.\n\nTienes la siguiente información de empresas (companies) en formato JSON: {{$empresas}} Cada empresa tiene propiedades como: Id, Name, Type (tipo), Price.\n\nTambién tienes la siguiente información de materiales (materials) en formato JSON: {{$materiales}} Cada material tiene propiedades como: Id, Name, Issue (tipo), Available (disponible).\n\nUsa esta información para responder la pregunta del usuario: {{$pregunta}} Responde de forma clara y útil.\n\nSi la pregunta no tiene relación con los datos, responde que no puedes ayudar.";

            // Inicializa plugins y kernel
            var dbPlugin = new DBPluginTestPG(config.GetConnectionString("CleanFixDB"));
            var empresasResponse = dbPlugin.GetAllEmpresas();
            var materialesResponse = dbPlugin.GetAllMaterials();

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

            // 1. Responde directamente si la petición es simple
            if (mensaje.Contains("lista empresas") || mensaje.Contains("todas las empresas") || mensaje.Trim().ToLower() == "empresas")
                return _empresasJson;
            if (mensaje.Contains("lista materiales") || mensaje.Contains("todos los materiales") || mensaje.Trim().ToLower() == "materiales")
                return _materialesJson;

            // 2. Solo llama a OpenAI si es lenguaje natural complejo
            var promptFunction = _kernel.CreateFunctionFromPrompt(_promptTemplate);
            var kernelArgs = new KernelArguments
            {
                ["empresas"] = _empresasJson,
                ["materiales"] = _materialesJson,
                ["pregunta"] = mensaje
            };

            try
            {
                var responseData = await promptFunction.InvokeAsync(_kernel, kernelArgs);
                var respuesta = responseData.GetValue<string>();
                Debug.WriteLine($"[AssistantService] Respuesta LLM: {respuesta}");
                return respuesta;
            }
            catch (Microsoft.SemanticKernel.HttpOperationException ex) when ((int)ex.StatusCode == 429)
            {
                return "Has superado el límite de peticiones. Intenta de nuevo en unos segundos.";
            }
        }
    }
}
